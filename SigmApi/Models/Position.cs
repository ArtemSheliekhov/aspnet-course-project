using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SigmApi.Models;

[Table("Position")]
public partial class Position
{
    [Key]
    public int PositionId { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [InverseProperty("Position")]
    public virtual ICollection<CompanyWorker> CompanyWorkers { get; set; } = new List<CompanyWorker>();
}
