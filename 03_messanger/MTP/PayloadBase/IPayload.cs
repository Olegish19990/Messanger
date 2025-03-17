namespace MTP.PayloadBase;

public interface IPayload
{
    public MemoryStream GetStream();
    public string PType { get; }
}
