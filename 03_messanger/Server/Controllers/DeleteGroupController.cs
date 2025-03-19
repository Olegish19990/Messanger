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
                db.Rooms.Where(g => g.Id == payload.Id).ExecuteDelete();
            }
            Console.WriteLine("group delete");

            Group group = activeConnectionsManager.groupsOnline.groups.Where(g => g.room.Id == payload.Id).FirstOrDefault();

            activeConnectionsManager.RemoveContectedGroup(group);




        }


    }
}
