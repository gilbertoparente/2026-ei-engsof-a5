using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProfileMAnager.Models;

public partial class Skill
{
    [Key]
    public int Idskill { get; set; }

    public string Nome { get; set; } = null!;

    public int? Idarea { get; set; }

    public virtual Areaprofissional? IdareaNavigation { get; set; }

    public virtual ICollection<Propostaskill> Propostaskills { get; set; } = new List<Propostaskill>();

    public virtual ICollection<Talentoskill> Talentoskills { get; set; } = new List<Talentoskill>();
}
