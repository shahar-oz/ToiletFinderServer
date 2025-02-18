using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToiletFinderServer.Models;

public partial class Rate
{
    [Key]
    public int Id { get; set; }

    [Column("Rate")]
    public int? Rate1 { get; set; }

    [Column("ToiletID")]
    public int ToiletId { get; set; }

    [ForeignKey("ToiletId")]
    [InverseProperty("Rates")]
    public virtual CurrentToilet Toilet { get; set; } = null!;
}
