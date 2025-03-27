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
using MTP.PayloadBase;
using Server.ServerSuccessResponce;

namespace Server.Controllers
{
    public class RegistrationController
    {
        private ActiveConnectionsManager activeConnectionsManager { get; set; }

        public void Registration<T>(ProtoMessage<T> pm, Client client, ActiveConnectionsManager activeConnectionsManager) where T : IPayload
        {
            this.activeConnectionsManager = activeConnectionsManager;

            RegistrationRequestPayload payload = pm.GetPayload() as RegistrationRequestPayload;

            User user = new User()
            {
                Login = payload.Login,
                Password = payload.Password
            };

            using (Db Db = new Db())
            {
                try
                {
                    Db.Users.Add(user);

                    Db.SaveChanges();

                    SuccessSender.Send(client, "Success registration");

                }
                catch (Exception ex)
                {
                    ErrorSender.SendError(client, ErrorCode.InvalidRequest);
                }
            }

        }
    }
}

