using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class Room
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int RoomTypeId { get; set; }

    public byte Status { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual RoomType RoomType { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
