using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProfileMAnager.Models;

public partial class Talento
{
    [Key]
    public int Idtalento { get; set; }

    public int Idutilizador { get; set; }

    public int Idcategoria { get; set; }

    public string? pais { get; set; }

    public decimal precohora { get; set; }

    public bool? Publico { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Experiencia> Experiencia { get; set; } = new List<Experiencia>();

    public virtual Categoriatalento IdcategoriaNavigation { get; set; } = null!;

    public virtual Utilizador IdutilizadorNavigation { get; set; } = null!;

    public virtual ICollection<Talentoskill> Talentoskills { get; set; } = new List<Talentoskill>();
}
