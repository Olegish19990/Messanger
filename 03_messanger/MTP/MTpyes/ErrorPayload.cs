using MTP.PayloadBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTypes
{
    public class ErrorPayload : JsonPayload
    {
        public string ErrorMessage { get; set; }

        public ErrorPayload(string MessageError)
        {
            this.ErrorMessage = MessageError;
        }
        public ErrorPayload() { }
        public override string GetJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public override MemoryStream GetStream()
        {
            string json = GetJson();

            byte[] bytes = Encoding.UTF8.GetBytes(json);

            MemoryStream memStream = new MemoryStream(bytes.Length);
            memStream.Write(bytes, 0, bytes.Length);

            return memStream;
        }
    }
}
