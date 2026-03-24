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

    // ADICIONADO:
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string? Pais { get; set; }

    public decimal Precohora { get; set; }

    public bool Publico { get; set; } = true;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Experiencia> Experiencia { get; set; } = new List<Experiencia>();

    public virtual Categoriatalento IdcategoriaNavigation { get; set; } = null!;

    public virtual Utilizador IdutilizadorNavigation { get; set; } = null!;

    public virtual ICollection<Talentoskill> Talentoskills { get; set; } = new List<Talentoskill>();
}