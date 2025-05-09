using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace edusync.Models;

public partial class Course
{
    [Key]
    public Guid id { get; set; }

    [StringLength(200)]
    public string title { get; set; } = null!;

    public string? description { get; set; }

    public Guid teacher_id { get; set; }

    [StringLength(500)]
    public string? thumbnail { get; set; }

    [StringLength(100)]
    public string? category { get; set; }

    public int duration { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }
}
