using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace edusync.Models;

[Index("Email", Name = "UQ__Users__A9D1053491DA832C", IsUnique = true)]
public partial class User
{
    [Key]
    public Guid UserId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(256)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(20)]
    public string Role { get; set; } = null!;
}
