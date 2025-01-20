using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToiletFinderServer.Models;

public partial class Status
{
    [Key]
    [Column("StatusID")]
    public int StatusId { get; set; }

    [StringLength(50)]
    public string? StatusDesc { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<CurrentToilet> CurrentToilets { get; set; } = new List<CurrentToilet>();
}
