using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
