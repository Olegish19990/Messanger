using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientsOnline
    {
        public List<Client> Clients { get; } = new List<Client>();

        public ClientsOnline() { }

        public ClientsOnline (List<Client> clients)
        {
            Clients = clients;
        }

        public void AddClient(Client client)
        {
            lock (Clients)
            {
                Clients.Add(client);
            }
        }

        public void RemoveClient(Client client)
        {
            lock (Clients)
            {
                Clients.Remove(client);
            }
        }
    }
}
