using System.ComponentModel.DataAnnotations;

namespace EducateApp.ViewModels.TypesOfTotals
{
    public class CreateTypeOfTotalViewModel
    {
        [Required(ErrorMessage = "Введите название аттестации")]
        [Display(Name = "название аттестации")]
        public string CertificateName { get; set; }
    }
}
