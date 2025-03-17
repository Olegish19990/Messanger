using MTP;
using MTP.PayloadBase;
using MTypes;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

const string host = "127.0.0.1";
const int port = 5500;

try
{
    Console.WriteLine("Press Enter to Connect");
    Console.ReadLine();

    using TcpClient tcpClient = new TcpClient(host, port);
    using NetworkStream netStream = tcpClient.GetStream();

    _ = ReceiveMessages(netStream);

    while (true)
    {
        Console.WriteLine("Action: \n1) Auth\n2) Message\n3)Registration");
        if (!int.TryParse(Console.ReadLine(), out int action))
        {
            Console.WriteLine("Invalid input. Enter a number.");
            continue;
        }

        switch (action)
        {
            case 1:
                Console.WriteLine("Login, Password:");
                string login = Console.ReadLine();
                string password = Console.ReadLine();
                await SendDynamicMessage(typeof(AuthRequestPayload), "auth", new object[] { login, password }, netStream);
                break;
            case 2:
                Console.WriteLine("Message, GroupId:");
                string message = Console.ReadLine();
                if (!int.TryParse(Console.ReadLine(), out int groupId))
                {
                    Console.WriteLine("Invalid group ID.");
                    continue;
                }
                await SendDynamicMessage(typeof(MessageRequestPayload), "message", new object[] { message, groupId }, netStream);
                break;
            case 3:
                Console.WriteLine("Registration: Login, password");
                string Reglogin = Console.ReadLine();
                string Regpassword = Console.ReadLine();
                await SendDynamicMessage(typeof(RegistrationRequestPayload), "reg", new object[] { Reglogin, Regpassword }, netStream);
                break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}

async Task SendDynamicMessage(Type payloadType, string action, object[] payloadArgs, NetworkStream netStream)
{
    object payloadInstance = Activator.CreateInstance(payloadType, payloadArgs);
    Type protoMessageType = typeof(ProtoMessage<>).MakeGenericType(payloadType);
    object protoMessageInstance = Activator.CreateInstance(protoMessageType);

    protoMessageType.GetProperty("Action")?.SetValue(protoMessageInstance, action);
    protoMessageType.GetMethod("SetPayload")?.Invoke(protoMessageInstance, new object[] { payloadInstance });

    using MemoryStream memStream = (MemoryStream)protoMessageType.GetMethod("GetStream")?.Invoke(protoMessageInstance, null);

    Console.WriteLine("Press Enter to Send");
    Console.ReadLine();
    Console.WriteLine($"Sending {memStream.Length} bytes...");

    await memStream.CopyToAsync(netStream);
}

async Task ReceiveMessages(NetworkStream netStream)
{
    ProtoMessageBuilder builder = new ProtoMessageBuilder(netStream);

    try
    {
        while (true)
        {
            object protoMessage = await Task.Run(() => builder.Receive());

            Console.WriteLine(protoMessage.GetType());

            switch (protoMessage)
            {
                case ProtoMessage<MessageRequestPayload> message:
                    MessageRequestPayload payloadMessage = message.GetPayload() as MessageRequestPayload;
                    Console.WriteLine($"Message: {payloadMessage.Message}");
                    break;
                case ProtoMessage<ErrorPayload> error:
                    ErrorPayload payloadError = error.GetPayload() as ErrorPayload;
                    Console.WriteLine($"Error: {payloadError.ErrorMessage}");
                    break;
              
    
            }
        }
    }
    catch (IOException ioEx)
    {
        Console.WriteLine($"Connection closed: {ioEx.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Receive error: {ex.Message}");
    }
}
