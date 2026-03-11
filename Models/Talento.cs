using System;
using System.Collections.Generic;

namespace ProfileMAnager.Models;

public partial class Talento
{
    public int Idtalento { get; set; }

    public int Idutilizador { get; set; }

    public int Idcategoria { get; set; }

    public string? Pais { get; set; }

    public decimal Precohora { get; set; }

    public bool? Publico { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Experiencium> Experiencia { get; set; } = new List<Experiencium>();

    public virtual Categoriatalento IdcategoriaNavigation { get; set; } = null!;

    public virtual Utilizador IdutilizadorNavigation { get; set; } = null!;

    public virtual ICollection<Talentoskill> Talentoskills { get; set; } = new List<Talentoskill>();
}
