using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToiletFinderServer.Models;

public partial class CurrentToilet
{
    [Key]
    public int ToiletId { get; set; }

    [Column("TLocation")]
    [StringLength(100)]
    public string? Tlocation { get; set; }

    public bool? Accessibility { get; set; }

    public double? Price { get; set; }

    [InverseProperty("Toilet")]
    public virtual Rate? Rate { get; set; }

    [InverseProperty("Toilet")]
    public virtual Review? Review { get; set; }
}
