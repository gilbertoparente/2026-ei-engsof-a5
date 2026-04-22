using System.ComponentModel.DataAnnotations;

namespace ProfileMAnager.Models;

public partial class Propostatrabalho
{
    [Key]
    public int Idproposta { get; set; }
    public int Idutilizador { get; set; }
    public int Idcliente { get; set; }
    public string Nome { get; set; } = null!;
    public int Idcategoria { get; set; }
    public int? Horastotais { get; set; }
    public string? Descricao { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Estado { get; set; } = "ABERTA";
    public virtual Categoriatalento? IdcategoriaNavigation { get; set; }
    public virtual Cliente? IdclienteNavigation { get; set; }
    public virtual Utilizador? IdutilizadorNavigation { get; set; }

    public virtual ICollection<Propostaskill> Propostaskills { get; set; } = new List<Propostaskill>();
    public virtual ICollection<PropostaTalento> PropostaTalentos { get; set; } = new List<PropostaTalento>();
}