using MTP.PayloadBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTypes
{
    public class MessageRequestPayload : JsonPayload
    {
        public string Message { get; set; }
        public string userId { get; set; }

        public int GroupId { get; set; }

        public MessageRequestPayload(string Message, int GroupId)
        {
            this.Message = Message;
            this.GroupId = GroupId;
        }
        public MessageRequestPayload() { }
        public MessageRequestPayload(string message) 
        {
            this.Message = message;
        }
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
