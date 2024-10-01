using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToiletFinderServer.Models;

public partial class Review
{
    [Column("Review")]
    [StringLength(250)]
    public string? Review1 { get; set; }

    [Key]
    [Column("ToiletID")]
    public int ToiletId { get; set; }

    [ForeignKey("ToiletId")]
    [InverseProperty("Review")]
    public virtual CurrentToilet Toilet { get; set; } = null!;
}
