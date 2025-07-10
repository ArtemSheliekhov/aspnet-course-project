using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SigmApi.Models;

[Table("CompanyWorker")]
public partial class CompanyWorker
{
    [Key]
    public int CompanyWorkerId { get; set; }

    public int? CompanyId { get; set; }

    public int? WorkerId { get; set; }

    [StringLength(255)]
    public string? Position { get; set; }

    public DateOnly? JoinedDate { get; set; }

    [ForeignKey("CompanyId")]
    [InverseProperty("CompanyWorkers")]
    public virtual Company? Company { get; set; }

    [ForeignKey("WorkerId")]
    [InverseProperty("CompanyWorkers")]
    public virtual Worker? Worker { get; set; }
}
