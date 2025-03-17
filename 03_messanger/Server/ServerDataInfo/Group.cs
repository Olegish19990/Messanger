using Autofac;
using MTP;
using MTP.PayloadBase;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ServerDataInfo
{
    public class Group
    {
     
        public Room room { get; set; }

        public List<Client> clients { get; set; } = new List<Client>();

        public void AddClientToGroup(Client client)
        {
            clients.Add(client);
        }
        public void RemoveClientFromGroup(Client client)
        {
            clients.Remove(client);
        }


        public void SendMessageToGroup<T>(ProtoMessage<T> protoMessage) where T : IPayload
        {
            foreach (Client client in clients)
            {
                client.ProtoMessageSend(protoMessage);
            }
        }
    }
}
