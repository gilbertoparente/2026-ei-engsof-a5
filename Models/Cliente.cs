using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProfileMAnager.Models;

public partial class Cliente
{
    [Key]
    public int Idcliente { get; set; }

    public int Idutilizador { get; set; }

    public string Nome { get; set; } = null!;

    public string? Pais { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Utilizador IdutilizadorNavigation { get; set; } = null!;

    public virtual ICollection<Propostatrabalho> Propostatrabalhos { get; set; } = new List<Propostatrabalho>();
}
