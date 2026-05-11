using System.ComponentModel.DataAnnotations;

namespace ProfileMAnager.Models.ViewModels
{
    public class PerfilViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nova Password (deixe em branco para manter)")]
        public string? NovaPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nova Password")]
        [Compare("NovaPassword", ErrorMessage = "As passwords não coincidem")]
        public string? ConfirmarPassword { get; set; }
    }
}