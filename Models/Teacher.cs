using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace edusync.Models;

[Index("email", Name = "UQ__Teachers__AB6E61640B991231", IsUnique = true)]
public partial class Teacher
{
    [Key]
    public Guid id { get; set; }

    [StringLength(100)]
    public string name { get; set; } = null!;

    [StringLength(255)]
    public string email { get; set; } = null!;

    [StringLength(20)]
    public string role { get; set; } = null!;

    [StringLength(500)]
    public string? avatar { get; set; }

    public string? bio { get; set; }

    public DateTime? created_at { get; set; }
}
