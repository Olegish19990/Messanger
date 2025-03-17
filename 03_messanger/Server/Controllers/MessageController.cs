using MTP.PayloadBase;
using MTP;
using MTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Models;
using Server.ServerDataInfo;
using Microsoft.EntityFrameworkCore;
using Server.ErrorHandling;

namespace Server.Controllers
{
    public class MessageController
    {


        private ActiveConnectionsManager activeConnectionsManager { get; set; }


        public void ProcessMessage<T>(ProtoMessage<T> pm, Client client, ActiveConnectionsManager activeConnectionsManager) where T : IPayload
        {
            this.activeConnectionsManager = activeConnectionsManager;

            MessageRequestPayload payload = pm.GetPayload() as MessageRequestPayload;

            if (client.user != null)
            {

                var targetGroup = activeConnectionsManager.groupsOnline.groups
                    .FirstOrDefault(g => g.room.Id == payload.GroupId);
       

                if (targetGroup is null || !client.user.Rooms.Any(r => r.Id == payload.GroupId))
                {
                    ErrorSender.SendError(client, ErrorCode.Forbidden);
                    return;
                }

                SendMessage(client,targetGroup,payload);

            }
            else
            {
                ErrorSender.SendError(client, ErrorCode.Unauthorized);
            }
        }

       


        public void SendMessage(Client client, Group targetGroup, MessageRequestPayload payload)
        {
            string message = $"{DateTime.Now} {client.user.Login}: {payload.Message}";

            ProtoMessage<MessageRequestPayload> protoMessage = new ProtoMessage<MessageRequestPayload>();
            MessageRequestPayload messageRequsetPayload = new MessageRequestPayload(message);
            protoMessage.SetPayload(messageRequsetPayload);
            protoMessage.Action = "message";
            targetGroup.SendMessageToGroup(protoMessage);
        }
    }
}


