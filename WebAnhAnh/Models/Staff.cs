using System;
using System.Collections.Generic;

namespace WebAnhAnh.Models;

public partial class Staff
{
    public string StaffId { get; set; } = null!;

    public string? UserName { get; set; }

    public string? StaffName { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual User? UserNameNavigation { get; set; }
}
