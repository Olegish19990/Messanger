using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using MTP;
using MTP.MTpyes;
using MTP.PayloadBase;
using MTypes;
using Server.Controllers;
using Server.ErrorHandling;
using Server.Models;
using Server.Routing;
using Server.ServerDataInfo;

namespace Server;

public class Client
{
    private TcpClient tcpClient;
    private NetworkStream netStream = null!;
    private Router router;
    public User? user = null;
    private ActiveConnectionsManager activeConnectionsManager { get; set; }

    public Client(TcpClient tcpClient, ActiveConnectionsManager activeConnectionsManager)
    {
        this.activeConnectionsManager = activeConnectionsManager;
        this.tcpClient = tcpClient;
        router = new Router(new List<Route>()
        {
            new Route("auth", typeof(AuthController), "Login"),
            new Route("message", typeof(MessageController), "ProcessMessage"),
            new Route("reg", typeof(RegistrationController), "Registration"),
            new Route("groupCreate", typeof(CreateGroupController), "CreateGroup"),
            new Route("groupDelete",typeof(DeleteGroupController),"DeleteGroup"),
            new Route("joinGroup"),typeof(JoinToGroupController),"JointToGroup"),
        });
    }


    public void Processing()
    {
        try
        {
            netStream = tcpClient.GetStream();

            ProtoMessageBuilder builder = new ProtoMessageBuilder(netStream);

            while (true)
            {

                // Принимаем сообщение как динамический объект
                try
                {
                    object protoMessage = builder.Receive();

                    //IProtoMessage protoMessage = ProtoMessageAnalyzer.Analyze(RawProtoMessage);
                    switch (protoMessage)
                    {
                        case ProtoMessage<AuthRequestPayload> authMsg:
                            router.Handle(authMsg, this, activeConnectionsManager);
                            break;

                        case ProtoMessage<MessageRequestPayload> msgMsg:
                            router.Handle(msgMsg, this, activeConnectionsManager);
                            break;
                        case ProtoMessage<RegistrationRequestPayload> regMsg:
                            router.Handle(regMsg, this, activeConnectionsManager);
                            break;
                        case ProtoMessage<GroupCreatePayload> groupCreate:
                            router.Handle(groupCreate, this, activeConnectionsManager);
                            break;
                        case ProtoMessage<GroupDeletePayload> groupDelete:
                            router.Handle(groupDelete, this, activeConnectionsManager);
                            break;

                        default:
                            throw new InvalidOperationException("Неизвестный тип сообщения");
                    }
                }
                catch
                {
                    netStream.Close();
                    Console.WriteLine("Connection close");
                    activeConnectionsManager.RemoveConectedClient(this);

                    break;
                }




            }


        }
        catch (Exception)
        {
            ErrorSender.SendError(this, ErrorCode.InvalidRequest);
            throw;
        }
    }

    public void ProtoMessageSend<T>(ProtoMessage<T> protmessage) where T : IPayload
    {
        MemoryStream memStream = protmessage.GetStream();
        memStream.CopyTo(netStream);
    }
}
