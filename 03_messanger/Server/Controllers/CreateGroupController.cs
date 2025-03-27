using MTP.PayloadBase;
using MTP;
using MTypes;
using Server.ErrorHandling;
using Server.ServerDataInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTP.MTpyes;
using Server.Models;
using Server.ServerSuccessResponce;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    public class CreateGroupController
    {
        private ActiveConnectionsManager activeConnectionsManager { get; set; }

        public void CreateGroup<T>(ProtoMessage<T> pm, Client client, ActiveConnectionsManager activeConnectionsManager) where T : IPayload
        {


            Group group = new Group();
            this.activeConnectionsManager = activeConnectionsManager;            
            var payload = pm.GetPayload() as GroupCreatePayload;
            if (payload == null || string.IsNullOrEmpty(payload.Title))
            {
                ErrorSender.SendError(client, ErrorCode.InvalidRequest);
                return;
            }


            if (client.user is null)
            {
                ErrorSender.SendError(client, ErrorCode.AuthorizedError);
                return;
            }

            try
            {
                using (var db = new Db())
                {
                    var roomType = db.RoomTypes.AsNoTracking().FirstOrDefault(t => t.Title == payload.roomType);

                    if (roomType == null)
                    {
                        ErrorSender.SendError(client, ErrorCode.NotFound);
                        return;
                    }

                    if (roomType.Title.Equals("private_room", StringComparison.OrdinalIgnoreCase))
                    {
                        if (payload.UsersId == null || payload.UsersId.Count != 1)
                        {
                            ErrorSender.SendError(client, ErrorCode.InvalidRequest);
                            return;
                        }
                    }

                    var roomDb = new Room
                    {
                        RoomTypeId = roomType.Id,
                        Status = 1,
                        Title = payload.Title,
                        AdminId = client.user.Id
                    };

                   
                    db.Users.Attach(client.user);
                    db.Rooms.Add(roomDb);

               

                    var users = db.Users.Where(u => payload.UsersId.Contains(u.Id)).ToList();
                    roomDb.Users = users;
                    roomDb.Users.Add(client.user);
                    db.SaveChanges();

                    group.room = roomDb;
                    group.clients.Add(client);

                    activeConnectionsManager.AddConnectedGroup(group);

                   
                   

                    SuccessSender.Send(client, "Group was created");


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); 
                ErrorSender.SendError(client, ErrorCode.InvalidRequest);
            }
        }

        
    }
}
