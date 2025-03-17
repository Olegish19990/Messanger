using MTP.PayloadBase;
using MTP;
using MTypes;
using Server.ErrorHandling;
using Server.ServerDataInfo;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Controllers
{
    public class RegistrationController
    {
        private ActiveConnectionsManager activeConnectionsManager { get; set; }

        public void Registration<T>(ProtoMessage<T> pm,Client client,ActiveConnectionsManager activeConnectionsManager) where T: IPayload
        {
            this.activeConnectionsManager = activeConnectionsManager;

            RegistrationRequestPayload payload = pm.GetPayload() as RegistrationRequestPayload;

            User user = new User()
            {
                Login = payload.Login,
                Password = payload.Password
            };

            using(Db db = new Db())
            {
                try
                {
                    db.Users.Add(user);

                    db.SaveChanges();

                    Console.WriteLine($"User has succesfulyy created with: \nLogin: {payload.Login} \nPassword: {user.Password} " +
                        $"\nId: {user.Id}");

                }
                catch(Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
    }
}

