using System;
using System.Collections.Generic;

namespace ProfileMAnager.Models;

public partial class Talentoskill
{
    public int Idtalento { get; set; }

    public int Idskill { get; set; }

    public int? Anosexperiencia { get; set; }

    public virtual Skill IdskillNavigation { get; set; } = null!;

    public virtual Talento IdtalentoNavigation { get; set; } = null!;
}
