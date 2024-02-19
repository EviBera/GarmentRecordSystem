using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        void Exit(ref bool exit);
    }
}
