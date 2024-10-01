using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToiletFinderServer.Models;

[Table("UTypes")]
public partial class Utype
{
    [Key]
    public int UserType { get; set; }

    [StringLength(100)]
    public string? TypeName { get; set; }

    [InverseProperty("UserTypeNavigation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
