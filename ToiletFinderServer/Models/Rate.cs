using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToiletFinderServer.Models;

public partial class Rate
{
    [Column("Rate")]
    public int? Rate1 { get; set; }

    [Key]
    [Column("ToiletID")]
    public int ToiletId { get; set; }

    [ForeignKey("ToiletId")]
    [InverseProperty("Rate")]
    public virtual CurrentToilet Toilet { get; set; } = null!;
}
