using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MTP;
using MTP.PayloadBase;
using MTypes;
using Server.ErrorHandling;
using Server.Models;
using Server.ServerDataInfo;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Server.Controllers;

internal class AuthController
{
    public void Login<T>(ProtoMessage<T> pm, Client client, ActiveConnectionsManager activeConnectionsManager) where T : IPayload
    {
        User user = client.user;
        StringBuilder groupsJoinsMessage = new StringBuilder();
        AuthRequestPayload? payload = pm.GetPayload() as AuthRequestPayload;
        if (payload != null)
        {
            using (Db db = new Db()) {
                var unauthUser = db.Users.Where(u => u.Login == payload.Login).Include(r=>r.Rooms).FirstOrDefault();
                if(unauthUser is not null && unauthUser.Password == payload.Password)
                {
                    client.user = unauthUser;

                    string connectionsMessage = ConnectClientToGroups(client, activeConnectionsManager);

                    SendConnectionsMessage(connectionsMessage,client);    
                    
                }
                else
                {
                    ErrorSender.SendError(client, ErrorCode.AuthorizedError);
                }
             }
            
        }
    }

    private string ConnectClientToGroups(Client client, ActiveConnectionsManager activeConnectionsManager)
    {
        StringBuilder connectionsMessage = new StringBuilder();

        var activeRooms = activeConnectionsManager.groupsOnline.groups.ToDictionary(g => g.room.Id, g => g);
        foreach (var room in client.user.Rooms)
        {

            if (activeRooms.TryGetValue(room.Id, out Group groupForJoin))
            {
                groupForJoin.AddClientToGroup(client);
                connectionsMessage.Append($"You are joined to group: {room.Id} {room.Title}");

            }
        }
        return connectionsMessage.ToString();

    }

    //TODO . Create a logic for send message from DB for chats.
    private void SendConnectionsMessage(string connectionsMessage, Client client)
    {
        var messagePayload = new MessageRequestPayload(
            string.IsNullOrEmpty(connectionsMessage) ? "You are not joined in any groups" : connectionsMessage
        );

        var message = new ProtoMessage<MessageRequestPayload>
        {
            Action = "message"
        };
        message.SetPayload(messagePayload);

        client.ProtoMessageSend(message);
    }

  


}
