using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SigmApi.Models;

[Table("Game")]
public partial class Game
{
    [Key]
    public int AppId { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    public int? CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    [InverseProperty("Games")]
    public virtual Company? Company { get; set; }

    [ForeignKey("AppId")]
    [InverseProperty("Apps")]
    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    [ForeignKey("AppId")]
    [InverseProperty("Apps")]
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
