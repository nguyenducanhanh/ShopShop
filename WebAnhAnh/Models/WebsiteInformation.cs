using System;
using System.Collections.Generic;

namespace WebAnhAnh.Models;

public partial class WebsiteInformation
{
    public int Id { get; set; }

    public int? PhoneNumber01 { get; set; }

    public int? PhoneNumber02 { get; set; }

    public string? Address { get; set; }

    public string? ImageLogo { get; set; }

    public string? StoreName { get; set; }

    public string? Email { get; set; }

    public string? ImageBanner01 { get; set; }

    public string? ImageBanner02 { get; set; }

    public string? ImageBanner03 { get; set; }
}
