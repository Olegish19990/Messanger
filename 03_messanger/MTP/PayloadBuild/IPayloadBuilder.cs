using MTP.PayloadBase;

namespace MTP.PayloadBuild;

public interface IPayloadBuilder<T>
    where T : IPayload
{
    public T? BuildFromStream(MemoryStream memStream);
}
