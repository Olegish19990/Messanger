using MTP.MTpyes;
using MTP.PayloadBase;
using MTP;
using Server.ErrorHandling;
using Server.Models;
using Server.ServerDataInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.ServerSuccessResponce;

namespace Server.Controllers
{
    public class DeleteGroupController
    {

        private ActiveConnectionsManager activeConnectionsManager;
        public void DeleteGroup<T>(ProtoMessage<T> pm, Client client, ActiveConnectionsManager activeConnectionsManager) where T : IPayload
        {
            this.activeConnectionsManager = activeConnectionsManager;

            GroupDeletePayload payload = pm.GetPayload() as GroupDeletePayload;

            using (Db db = new Db())
            {
                var room = db.Rooms.Where(g => g.Id == payload.Id).FirstOrDefault();

                if(room is null)
                {
                    ErrorSender.SendError(client, ErrorCode.NotFound);
                    return;
                }

                if (room.AdminId != client.user.Id)
                {
                    ErrorSender.SendError(client, ErrorCode.Forbidden);
                    return;
                }


                db.Rooms.Remove(room);
                var groupForDelete = activeConnectionsManager.groupsOnline.groups.Where(g => g.room.Id == room.Id).FirstOrDefault();
                activeConnectionsManager.RemoveContectedGroup(groupForDelete);
                db.SaveChanges();
            }
            Console.WriteLine("group delete");
            SuccessSender.Send(client, "group was deleted");
            Group group = activeConnectionsManager.groupsOnline.groups.Where(g => g.room.Id == payload.Id).FirstOrDefault();

            activeConnectionsManager.RemoveContectedGroup(group);




        }


    }
}
