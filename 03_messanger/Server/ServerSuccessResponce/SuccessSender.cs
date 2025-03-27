using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTP;
using MTP.MTpyes;

namespace Server.ServerSuccessResponce
{
    public static class SuccessSender
    {
        public static void Send(Client cleint, string responceText = "The operation was successful") 
        {
            SuccessPayload successPayload = new SuccessPayload();

            successPayload.SuccessText = responceText;

            ProtoMessage<SuccessPayload> successMessage = new ProtoMessage<SuccessPayload>()
            {
                Action = "successResp"
            };


            successMessage.SetPayload(successPayload);
            cleint.ProtoMessageSend(successMessage);
        }
    }
}
