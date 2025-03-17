using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ServerDataInfo
{
    public class ActiveConnectionsManager
    {
        public ActiveConnectionsManager()
        { }


        public ActiveConnectionsManager(List<Room> rooms)
        {
           
            groupsOnline = new GroupsOnline(rooms);
            clientsOnline = new ClientsOnline();
        }

        public ClientsOnline clientsOnline { get; set; }
        public GroupsOnline groupsOnline { get; set; }

        public void AddConnectedGroup(Group group)
        {
            groupsOnline.AddGroup(group);
        }
        public void AddConectedClient(Client client)
        {
            clientsOnline.AddClient(client);
        }

        public void RemoveConectedClient(Client client)
        {
            clientsOnline.RemoveClient(client);
        }

        public void RemoveContectedGroup(Group group)
        {
            groupsOnline.RemoveGroup(group);
        }
    }
}
