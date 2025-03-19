using MTP.PayloadBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTP.MTpyes
{
    public class GroupCreatePayload : JsonPayload
    {
        public string Title { get; set; }

        public string RoomType { get; set; }

        public GroupCreatePayload(string title, string roomType)
        {
            Title = title;
            RoomType = roomType;
        }
        public GroupCreatePayload() { }
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
