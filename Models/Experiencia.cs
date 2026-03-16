using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProfileMAnager.Models;

public partial class Experiencia
{
    [Key]
    public int Idexperiencia { get; set; }

    public int Idtalento { get; set; }

    public string Empresa { get; set; } = null!;

    public int Anoinicio { get; set; }

    public int? Anofim { get; set; }

    public string? Descricao { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Talento IdtalentoNavigation { get; set; } = null!;
}
