using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SigmApi.Models;

[Table("Company")]
public partial class Company
{
    [Key]
    public int CompanyId { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    public int? FoundedYear { get; set; }

    [StringLength(255)]
    public string? Headquarters { get; set; }

    [InverseProperty("Company")]
    public virtual ICollection<CompanyWorker> CompanyWorkers { get; set; } = new List<CompanyWorker>();

    [InverseProperty("Company")]
    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
