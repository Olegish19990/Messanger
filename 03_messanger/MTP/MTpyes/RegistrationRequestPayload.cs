using MTP.PayloadBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTypes
{
    public class RegistrationRequestPayload : JsonPayload
    {
        public string Login { get; set; }
        public string Password { get; set; } //hash

        public RegistrationRequestPayload() { }
        public RegistrationRequestPayload(string login, string password)
        {
            Login = login;
            Password = password;
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
