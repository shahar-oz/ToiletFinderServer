using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToiletFinderServer.Models;

public partial class CurrentToiletsPhoto
{
    [Key]
    public int PhotoId { get; set; }

    public int? ToiletId { get; set; }

    [ForeignKey("ToiletId")]
    [InverseProperty("CurrentToiletsPhotos")]
    public virtual CurrentToilet? Toilet { get; set; }
}
