using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProfileMAnager.Models;

public partial class Utilizador
{
    [Key]
    public int Idutilizador { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Propostatrabalho> Propostatrabalhos { get; set; } = new List<Propostatrabalho>();

    public virtual ICollection<Talento> Talentos { get; set; } = new List<Talento>();
}
