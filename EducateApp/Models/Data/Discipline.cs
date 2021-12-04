using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EducateApp.Models.Data
{
    public class Discipline
    {
        // Key - поле первичный ключ
        // DatabaseGenerated(DatabaseGeneratedOption.Identity) - поле автоинкреметное
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        
        [Display(Name = "Индекс проф. модуля")]
        public string IndexProfModule { get; set; }

        [Display(Name = "Проф. модуль")]
        public string ProfModule { get; set; }

        [Required(ErrorMessage = "Введите индекс")]
        [Display(Name = "Индекс")]
        public string Index { get; set; }

        [Required(ErrorMessage = "Введите название")]
        [Display(Name = "Дисциплина")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите краткое название")]
        [Display(Name = "Краткое название")]
        public string ShortName { get; set; }
        //  внешний ключ
        [Required]
        public string IdUser { get; set; }

        // Навигационные свойства
        // свойство нужно для более правильного отображения данных в представлениях
        [ForeignKey("IdUser")]
        public User User { get; set; }
    }
}
