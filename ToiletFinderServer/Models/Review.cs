using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToiletFinderServer.Models;

public partial class Review
{
    [Key]
    public int ReviewId { get; set; }

    [Column("Review")]
    [StringLength(250)]
    public string? Review1 { get; set; }

    [Column("ToiletID")]
    public int ToiletId { get; set; }

    [ForeignKey("ToiletId")]
    [InverseProperty("Reviews")]
    public virtual CurrentToilet Toilet { get; set; } = null!;
}
