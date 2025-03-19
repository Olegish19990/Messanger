using MTP.PayloadBase;
using MTP;
using MTypes;
using Server.ErrorHandling;
using Server.ServerDataInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTP.MTpyes;
using Server.Models;

namespace Server.Controllers
{
    public class CreateGroupController
    {
        private ActiveConnectionsManager activeConnectionsManager;
        public void CreateGroup<T>(ProtoMessage<T> pm, Client client, ActiveConnectionsManager activeConnectionsManager) where T : IPayload
        {
            this.activeConnectionsManager = activeConnectionsManager;

            GroupCreatePayload payload = pm.GetPayload() as GroupCreatePayload;

            using(Db db = new Db()) 
            {
                RoomType roomType = db.RoomTypes.Where(t => t.Title == payload.RoomType).FirstOrDefault();
                if (roomType == null)
                {
                    ErrorSender.SendError(client, ErrorCode.NotFound);
                    return;
                }
                Room roomDb = new Room()
                {

                    RoomTypeId = roomType.Id,
                    Status = 1,
                    Title = payload.Title,
                };

                db.Users.Attach(client.user);
                roomDb.Users.Add(client.user);



                db.Rooms.Add(roomDb);
            
                db.SaveChanges();

                Console.WriteLine($"Group created: {roomDb.Id}\t{roomDb.Title}\t{roomDb.Users.First()}");

                Group group = new Group()
                {
                    room = roomDb,
                };
                group.clients.Add(client);

                activeConnectionsManager.AddConnectedGroup(group);
            }
           
        }

    }
}
