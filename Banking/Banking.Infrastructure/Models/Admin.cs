using System;
using System.Collections.Generic;

namespace Banking.Infrastructure.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public int? UserId { get; set; }

    public string? PhotoUrl { get; set; }

    public string Phone { get; set; } = null!;

    public bool Gender { get; set; }

    public string Position { get; set; } = null!;

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
