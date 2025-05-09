using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace edusync.Models;

public partial class Assessment
{
    [Key]
    public Guid id { get; set; }

    [StringLength(255)]
    public string title { get; set; } = null!;

    public string? description { get; set; }

    public Guid course_id { get; set; }

    public string? questions { get; set; }

    public int time_limit { get; set; }

    public int pass_score { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }
}
