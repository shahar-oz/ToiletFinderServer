using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToiletFinderServer.Models;

[Table("Sanitman")]
public partial class Sanitman
{
    [StringLength(100)]
    public string? Servicezone { get; set; }

    [StringLength(100)]
    public string? Username { get; set; }

    [StringLength(100)]
    public string? Pass { get; set; }

    [Key]
    [StringLength(100)]
    public string Email { get; set; } = null!;
}
