using System;
using System.Collections.Generic;

namespace WebAnhAnh.Models;

public partial class User
{
    public string UserName { get; set; } = null!;

    public string? Password { get; set; }

    public byte? Type { get; set; }

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
