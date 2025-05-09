using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace edusync.Models;

[Index("email", Name = "UQ__Students__AB6E61641899CD17", IsUnique = true)]
public partial class Student
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

    public string? assessment_ids { get; set; }

    public string? course_ids { get; set; }

    public string? results { get; set; }

    public int? num_courses_completed { get; set; }

    public int? num_assessments_attended { get; set; }
}
