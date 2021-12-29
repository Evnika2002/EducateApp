using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducateApp.ViewModels.Disciplines
{
    public class SortDisciplineViewModel
    {
        public DisciplineSortState IndexProfModuleSort { get; private set; }    // введенный код
        public DisciplineSortState ProfModuleSort { get; private set; }    // введенное имя
        public DisciplineSortState IndexSort { get; private set; }    // введенное имя
        public DisciplineSortState NameSort { get; private set; }    // введенное имя
        public DisciplineSortState ShortNameSort { get; private set; }     // текущее значение сортировки
        public DisciplineSortState Current { get; private set; }     // текущее значение сортировки

        public SortDisciplineViewModel(DisciplineSortState sortOrder)
        {
            IndexProfModuleSort = sortOrder == DisciplineSortState.IndexProfModuleAsc ?
                DisciplineSortState.IndexProfModuleDesc : DisciplineSortState.IndexProfModuleAsc;

            ProfModuleSort = sortOrder == DisciplineSortState.ProfModuleAsc ?
               DisciplineSortState.ProfModuleDesc : DisciplineSortState.ProfModuleAsc;
            IndexSort = sortOrder == DisciplineSortState.IndexAsc ?
                DisciplineSortState.IndexDesc : DisciplineSortState.IndexAsc;
            NameSort = sortOrder == DisciplineSortState.NameAsc ?
               DisciplineSortState.NameDesc : DisciplineSortState.NameAsc;
            ShortNameSort = sortOrder == DisciplineSortState.ShortNameAsc ?
                DisciplineSortState.ShortNameDesc : DisciplineSortState.ShortNameAsc;
            Current = sortOrder;
        }
    }
}
