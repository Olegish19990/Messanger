using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTP
{
    public interface IProtoMessage
    {
        string? Action { get; set; }
        MemoryStream? PayloadStream { get; }
        int PaylodLength { get; }
    }

}
