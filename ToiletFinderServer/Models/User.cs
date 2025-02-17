using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToiletFinderServer.Models;

[Index("Email", Name = "UQ__Users__A9D1053424FD8580", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string? Username { get; set; }

    [StringLength(100)]
    public string? Pass { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    public int? UserType { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<CurrentToilet> CurrentToilets { get; set; } = new List<CurrentToilet>();

    [ForeignKey("UserType")]
    [InverseProperty("Users")]
    public virtual Utype? UserTypeNavigation { get; set; }
}
