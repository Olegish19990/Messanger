
using System.Text.Json;
using MTP.PayloadBase;

namespace MTP.PayloadBuild;

internal class JsonPayloadBuilder<T> : IPayloadBuilder<T>
    where T : IPayload
{
    public T? BuildFromStream(MemoryStream memStream)
    {
        using StreamReader sr = new StreamReader(memStream);

        string json = sr.ReadToEnd();

        return JsonSerializer.Deserialize<T>(json);
    }
}
