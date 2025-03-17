using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTypes
{
    public class RegistrationRequestPayload
    {
        public string Login { get; set; }
        public string Password { get; set; } //hash

    }
}
