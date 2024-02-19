namespace ConsoleBasedUI.Services
{
    internal interface IHandleMainMenu
    {
        void AddGarment();
        void UpdateGarment();
        void DeleteGarment();
        void SearchGarment();
        void SortGarments();
        void SaveGarmentsToFile();
        void LoadGarmentsFromFile();
        void DisplayTheWholeCollection();
        void Exit(ref bool exit);
    }
}
