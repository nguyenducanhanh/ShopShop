using System;
using System.Collections.Generic;

namespace WebAnhAnh.Models;

public partial class KhachHang
{
    public string MaKhanhHang { get; set; } = null!;

    public string? UserName { get; set; }

    public string? TenKhachHang { get; set; }

    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }

    public byte? LoaiKhachHang { get; set; }

    public virtual User? UserNameNavigation { get; set; }
}
