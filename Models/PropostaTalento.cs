using System;

namespace ProfileMAnager.Models
{
    public class PropostaTalento
    {
        public int Id { get; set; }
        public int Idproposta { get; set; }
        public int Idtalento { get; set; }
        public DateTime? Datainicio { get; set; }
        public DateTime? Datafim { get; set; }
        public string Estado { get; set; } = "CANDIDATO";

        public virtual Propostatrabalho Proposta { get; set; } = null!;
        public virtual Talento Talento { get; set; } = null!;
    }
}