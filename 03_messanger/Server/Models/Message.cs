using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class Message
{
    public int Id { get; set; }

    public string? Content { get; set; }

    public int UserId { get; set; }

    public int RoomId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
