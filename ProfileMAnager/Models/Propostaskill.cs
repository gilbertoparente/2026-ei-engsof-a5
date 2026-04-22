namespace ProfileMAnager.Models;

public partial class Propostaskill
{
    public int Idproposta { get; set; }
    public int Idskill { get; set; }

    public int? Anosminimosexperiencia { get; set; }

    public virtual Propostatrabalho IdpropostaNavigation { get; set; } = null!;
    public virtual Skill IdskillNavigation { get; set; } = null!;
}