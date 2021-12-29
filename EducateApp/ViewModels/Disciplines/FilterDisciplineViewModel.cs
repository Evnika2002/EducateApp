namespace EducateApp.ViewModels.Disciplines
{
    public class FilterDisciplineViewModel
    {
        public string SelectedIndexProfModule { get; private set; }    // введенный код
        public string SelectedProfModule { get; private set; }    // введенное имя
        public string SelectedIndex { get; private set; }    // введенное имя
        public string SelectedName { get; private set; }    // введенное имя
        public string SelectedShortName { get; private set; }    // введенное имя

        public FilterDisciplineViewModel(
            string indexProfModule, string profModule, string index, string name, string shortName)
        {
            SelectedIndexProfModule = indexProfModule;
            SelectedProfModule = profModule;
            SelectedIndex = index;
            SelectedName = name;
            SelectedShortName = shortName;
        }
    }
}
