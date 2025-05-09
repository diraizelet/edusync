using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace edusync.Models;

public partial class Result
{
    [Key]
    public Guid ResultId { get; set; }

    public Guid AssessmentId { get; set; }

    public Guid UserId { get; set; }

    public int Score { get; set; }

    public DateTime AttemptDate { get; set; }
}
