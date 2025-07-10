using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SigmApi.Models;

[Table("Worker")]
public partial class Worker
{
    [Key]
    public int WorkerId { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string Role { get; set; } = null!;

    [InverseProperty("Worker")]
    public virtual ICollection<CompanyWorker> CompanyWorkers { get; set; } = new List<CompanyWorker>();
}
