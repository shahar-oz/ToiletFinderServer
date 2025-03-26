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
    [StringLength(1000)]
    public string? Tlocation { get; set; }

    public bool? Accessibility { get; set; }

    public double? Price { get; set; }

    [Column("StatusID")]
    public int? StatusId { get; set; }

    [Column("GoogleMapsID")]
    [StringLength(1000)]
    public string GoogleMapsId { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public int? UserId { get; set; }

    [InverseProperty("Toilet")]
    public virtual ICollection<CurrentToiletsPhoto> CurrentToiletsPhotos { get; set; } = new List<CurrentToiletsPhoto>();

    [InverseProperty("Toilet")]
    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();

    [InverseProperty("Toilet")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [ForeignKey("StatusId")]
    [InverseProperty("CurrentToilets")]
    public virtual Status? Status { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("CurrentToilets")]
    public virtual User? User { get; set; }
}
