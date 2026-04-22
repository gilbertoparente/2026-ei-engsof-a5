using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProfileMAnager.Models;

public partial class Areaprofissional
{
    [Key]
    public int Idarea { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
}
