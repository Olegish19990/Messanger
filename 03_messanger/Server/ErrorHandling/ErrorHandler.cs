using MTP;
using MTypes;
using Server;

namespace Server.ErrorHandling;

    public class ErrorSender
    {
        public static void SendError(Client client, ErrorCode errorCode)
        {
            var errorResponse = new ErrorResponse(errorCode);
            var errorPayload = new ErrorPayload(errorResponse.Message);
            var errorMessage = new ProtoMessage<ErrorPayload>
            {
                Action = "error"
            };
            errorMessage.SetPayload(errorPayload);
            client.ProtoMessageSend(errorMessage);
        }
    }
