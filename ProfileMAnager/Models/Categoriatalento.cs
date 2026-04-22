using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProfileMAnager.Models;

public partial class Categoriatalento
{
    [Key]
    public int Idcategoria { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Propostatrabalho> Propostatrabalhos { get; set; } = new List<Propostatrabalho>();

    public virtual ICollection<Talento> Talentos { get; set; } = new List<Talento>();
}
