using System.Net.Sockets;
using MTP.PayloadBase;
using MTP;
using System.Reflection;
using MTypes;
using MTP.MTpyes;
public class ProtoMessageBuilder
{

    private NetworkStream netStream;
    private MemoryStream memStream;
    public ProtoMessageBuilder(NetworkStream netStream)
    {
        this.netStream = netStream;
    }
    public object Receive()
    {
        int readingSize = ConvertToInt(ReadBytes(ProtoMessage<IPayload>.MESSAGE_LEN_LABLE_SIZE));

        memStream = new MemoryStream(readingSize);
        memStream.Write(ReadBytes(readingSize), 0, readingSize);
        memStream.Position = 0;

        using StreamReader sr = new StreamReader(memStream);

        Type payloadType = getPay(memStream).GetType();
        Type genericType = typeof(ProtoMessage<>).MakeGenericType(payloadType);
        object protoMessage = Activator.CreateInstance(genericType);

        MethodInfo extractMetadataMethod = typeof(ProtoMessageBuilder)
            .GetMethod(nameof(ExtractMetadata), BindingFlags.NonPublic | BindingFlags.Instance)!
            .MakeGenericMethod(payloadType);
        extractMetadataMethod.Invoke(this, new object[] { protoMessage, sr });

        MethodInfo extractPayloadStreamMethod = typeof(ProtoMessageBuilder)
            .GetMethod(nameof(ExtractPayloadStream), BindingFlags.NonPublic | BindingFlags.Instance)!
            .MakeGenericMethod(payloadType);
        extractPayloadStreamMethod.Invoke(this, new object[] { protoMessage });

      
        MethodInfo injectPayloadBuilderMethod = genericType.GetMethod("InjectPayloadBuilder");
        injectPayloadBuilderMethod?.Invoke(protoMessage, null);

        return protoMessage;
    }
    private IPayload getPay(MemoryStream m)
    {
        StreamReader reader = new StreamReader(m);
        reader.BaseStream.Position = 0;
        string Action = reader.ReadLine();
        IPayload paytp = Action switch
        {
            "auth" => new AuthRequestPayload(),
            "message" => new MessageRequestPayload(),
            "error" => new ErrorPayload(),
            "reg" => new RegistrationRequestPayload(),
            "groupCreate" => new GroupCreatePayload(),
            "groupDelete" => new GroupDeletePayload()
        };

        return paytp;
    
    }
    private void ExtractMetadata<T>(ProtoMessage<T> pm, StreamReader sr)
        where T : IPayload
    {
        sr.BaseStream.Position = 0;

        pm.Action = sr.ReadLine();


        string? headerLine;
        while(! string.IsNullOrEmpty(headerLine = sr.ReadLine()))
            pm.SetHeader(headerLine);
    }

    private void ExtractPayloadStream<T>(ProtoMessage<T> pm)
        where T : IPayload
    {
        int payloadLength = pm.PaylodLength;

        memStream.Seek(-payloadLength, SeekOrigin.End);

        pm.PayloadStream = new MemoryStream(payloadLength);
        memStream.CopyTo(pm.PayloadStream);
        pm.PayloadStream.Position = 0;
    }

    private byte[] ReadBytes(int count)
    {
        byte[] bytes = new byte[count];
        
        netStream.ReadExactly(bytes, 0, count);
     
        return bytes;
    }

    private int ConvertToInt(byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);

        return BitConverter.ToInt32(bytes, 0);
    }
}
