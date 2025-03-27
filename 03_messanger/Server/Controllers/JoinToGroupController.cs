using MTP.PayloadBase;
using MTP;
using Server.ServerDataInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTP.MTpyes;
using Server.Models;
using Server.ErrorHandling;
using Server.ServerSuccessResponce;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    public class JoinToGroupController
    {
        private ActiveConnectionsManager activeConnectionsManager { get; set; }

        public void JoinToGroup<T>(ProtoMessage<T> pm, Client client, ActiveConnectionsManager activeConnectionsManager) where T : IPayload
        {
            this.activeConnectionsManager = activeConnectionsManager;

            JoinToGroupPayload payload = pm.GetPayload() as JoinToGroupPayload;

            if(payload == null || client.user == null)
            {
                ErrorSender.SendError(client, ErrorCode.Unauthorized);
                return;
            }

            using(Db db = new Db())
            {
                db.Users.Attach(client.user);

                var targetRoom = db.Rooms.Include(r => r.RoomType)
                    .Where(r => r.Id == payload.TargetGroupId).FirstOrDefault();

                if (targetRoom == null)
                {
                    ErrorSender.SendError(client, ErrorCode.NotFound);
                    return;
                }
                if (targetRoom.RoomType.Title == "private_room")
                {
                    ErrorSender.SendError(client, ErrorCode.Forbidden);
                    return;
                }
               

                targetRoom.Users.Add(client.user);

                db.SaveChanges();
                SuccessSender.Send(client);
            }
        }

    }
}
