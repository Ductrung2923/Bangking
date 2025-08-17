using System;
using System.Collections.Generic;

namespace Banking.Infrastructure.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public int? UserId { get; set; }

    public string Position { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public bool Gender { get; set; }

    public string? PhotoUrl { get; set; }

    public DateOnly HireDate { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
