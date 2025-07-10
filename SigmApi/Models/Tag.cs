using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SigmApi.Models;

[Table("Tag")]
public partial class Tag
{
    [Key]
    public int TagId { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [ForeignKey("TagId")]
    [InverseProperty("Tags")]
    public virtual ICollection<Game> Apps { get; set; } = new List<Game>();
}
