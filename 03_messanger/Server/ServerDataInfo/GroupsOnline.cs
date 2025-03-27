using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ServerDataInfo
{
    public class GroupsOnline
    {
        public List<Group> groups { get; set; } = new List<Group>();
        public GroupsOnline() { }
        public GroupsOnline(List<Group> groups)
        {
            this.groups = groups;
        }
        public GroupsOnline(List<Room> rooms)
        {
            foreach (var item in rooms)
            {
                Group group = new Group();
                group.room = item;
                groups.Add(group);
                Console.WriteLine($"Group connected: Id: {group.room.Id}\nTitle: {group.room.Title}");
                foreach (User user in group.room.Users)
                {
                    Console.WriteLine($"{user.Login}\n{user.Password}");
                }
            }


        }
        public void AddGroup(Group group)
        {
            lock (groups)
            {
                groups.Add(group);
            }
        }

        public void RemoveGroup(Group group)
        {
            lock (groups)
            {
                groups.Remove(group);
            }
        }

    }
}
