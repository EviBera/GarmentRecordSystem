using ConsoleBasedUI.UI;
using GarmentRecordSystem.Models;
using GarmentRecordSystem.Services;

namespace ConsoleBasedUI.Services
{
    public class HandleMainMenuOptions : IHandleMainMenu
    {
        private readonly IJsonHandlerService _jsonHandlerService;
        private readonly IGarmentService _garmentService;
        private readonly Display _display;
        private readonly Input _input;

        public HandleMainMenuOptions()
        {
            _jsonHandlerService = new JsonHandlerService();
            _garmentService = new GarmentService(_jsonHandlerService);
            _display = new Display();
            _input = new Input();
        }


        public void AddGarment()
        {
            _display.PrintMessage("\nTo add a new garment you have to specify the following (please, type in):\n");

            var brandName = RegisterString("Brand name");
            var purchaseDate = RegisterPurchaseDate();
            var color = RegisterString("Color");
            var size = RegisterSize();

            try
            {
                _garmentService.AddGarment(new Garment(brandName, purchaseDate, color, size));
                _display.PrintColorMessage("\nGarment added successfully. (Not saved into file yet.)", ConsoleColor.DarkGreen);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void UpdateGarment()
        {
            Garment? oldGarment = default;

            while (oldGarment == null)
            {
                _display.PrintMessage("\nEnter the ID of the garment that needs an update: ");
                oldGarment = GetGarment();
            }
                        
            var newBrandName = UpdateBrandName(oldGarment);
            var newColor = UpdateColor(oldGarment);
            var newPurchase = UpdatePurchase(oldGarment);
            var newSize = UpdateSize(oldGarment);

            try
            {
                _garmentService.UpdateGarment(oldGarment.GarmentID, newBrandName, newPurchase, newColor, newSize);
                _display.PrintColorMessage("\nGarment udated successfully. (Not saved into file yet.)", ConsoleColor.DarkGreen);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteGarment()
        {
            Garment? needlessGarment = default;
            while (needlessGarment == null)
            {
                _display.PrintMessage("\nWhich garment would you like to delete? Enter ID: ");
                needlessGarment = GetGarment();
            }
            
            try
            {
                _garmentService.DeleteGarment(needlessGarment.GarmentID);
                _display.PrintColorMessage("\nGarment deleted successfully. (Changes are not saved into file yet.)", ConsoleColor.DarkGreen);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SearchGarment()
        {
            Garment? searchedGarment = default;
            while(searchedGarment == null)
            {
                _display.PrintMessage("\nWhich garment would you like to see? Enter ID: ");
                searchedGarment = GetGarment();
            }

            _display.PrintMessage("\nThe garment you are looking for:");
            _display.PrintMessage(searchedGarment.ToString());
        }

        public void SortGarments()
        {
            List<Garment> sortedList = new();
            bool isValid = false;

            _display.PrintMessage("\nYou can select which factor use for sorting.");
            _display.PrintMessage("1. Sort by brand name");
            _display.PrintMessage("2. Sort by date of purchase");
            _display.PrintMessage("3. Sort by size");
            _display.PrintMessage("4. Sort by color");
            _display.PrintMessage("Enter your choice: ");

            string? answer = _input.GetInputFromUser();

            switch (answer)
            {
                case "1":
                    sortedList = _garmentService.SortGarments(SortGarmentsCriteria.BrandName);
                    isValid = true;
                    break;
                case "2":
                    sortedList = _garmentService.SortGarments(SortGarmentsCriteria.PurchaseDate);
                    isValid = true;
                    break;
                case "3":
                    sortedList = _garmentService.SortGarments(SortGarmentsCriteria.Size);
                    isValid = true;
                    break;
                case "4":
                    sortedList = _garmentService.SortGarments(SortGarmentsCriteria.Color);
                    isValid = true;
                    break;
                default:
                    _display.PrintColorMessage("\nInvalid option. Please select a valid number (1 - 4).", ConsoleColor.Red);
                    break;
            }

            if (isValid)
            {
                _display.PrintMessage("\nThe garments sorted by your choice: \n");
                foreach (Garment g in sortedList)
                {
                    _display.PrintMessage(g.ToString());
                }
            }
        }

        public void SaveGarmentsToFile()
        {
            bool isValid = false;
            string? path = String.Empty;
            while (!isValid)
            {
                _display.PrintMessage("\nEnter file path for saving data: ");
                path = _input.GetInputFromUser();

                if (string.IsNullOrWhiteSpace(path))
                {
                    _display.PrintColorMessage("The file path cannot be empty. Please enter a valid path.", ConsoleColor.Red);
                }
                else if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    _display.PrintColorMessage("The file path contains invalid characters. Please enter a valid path.", ConsoleColor.Red);
                }
                else if (!path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    _display.PrintColorMessage("The file must be a .json file. Please enter a valid file path ending with '.json'.", ConsoleColor.Red);
                }
                else
                {
                    isValid = true;
                }
            }

            try
            {
                _jsonHandlerService.SaveToFile(path, _garmentService.GetAllGarments());
                _display.PrintColorMessage("File saved successfully.", ConsoleColor.DarkGreen);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void LoadGarmentsFromFile()
        {
            bool isValid = false;
            string? path = String.Empty;
            while (!isValid)
            {
                _display.PrintMessage("\nEnter required file path: ");
                path = _input.GetInputFromUser();

                if (string.IsNullOrWhiteSpace(path))
                {
                    _display.PrintColorMessage("The file path cannot be empty. Please enter a valid path.", ConsoleColor.Red);
                }
                else if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    _display.PrintColorMessage("The file path contains invalid characters. Please enter a valid path.", ConsoleColor.Red);
                }
                else if (!File.Exists(path))
                {
                    _display.PrintColorMessage("The file does not exist. Please enter a valid path to an existing file.", ConsoleColor.Red);
                }
                else if (!path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    _display.PrintColorMessage("The file must be a .json file. Please enter a valid file path ending with '.json'.", ConsoleColor.Red);
                }
                else
                {
                    isValid = true;
                }
            }

            try
            {
                _garmentService.LoadGarments(path);
                _display.PrintColorMessage("File loaded successfully.", ConsoleColor.DarkGreen);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DisplayTheWholeCollection()
        {
            _display.PrintMessage("Garments: ");
            foreach (var garment in _garmentService.GetAllGarments())
            {
                _display.PrintMessage(garment.ToString());
            }

        }

        public void Exit(ref bool exit)
        {
            exit = true;
        }

        private string RegisterString(string arg)
        {
            string? data = string.Empty;
            bool isValid = false;

            while (!isValid)
            {
                _display.PrintMessage(arg + ": ");
                data = _input.GetInputFromUser();

                if (string.IsNullOrWhiteSpace(data))
                {
                    _display.PrintColorMessage(arg + " cannot be empty. Please enter valid data.", ConsoleColor.Red);
                }

                else if (data.Length > 50)
                {
                    _display.PrintColorMessage(arg + " cannot exceed 50 characters. Please enter a shorter text.", ConsoleColor.Red);
                }

                else
                {
                    int specialCharCount = data.Count(c => Path.GetInvalidFileNameChars().Contains(c));
                    if (specialCharCount > 1)
                    {
                        _display.PrintColorMessage(arg + " can contain at most one special character. Please revise your input.", ConsoleColor.Red);
                    }
                    else
                    {
                        // If all checks pass
                        isValid = true;
                    }
                }
            }

            return data;
        }

        private DateOnly RegisterPurchaseDate()
        {
            DateOnly purchaseDate = default;
            bool isValidDate = false;

            while (!isValidDate)
            {
                _display.PrintMessage("Purchase date (yyyy-mm-dd): ");
                var dateString = _input.GetInputFromUser();

                isValidDate = DateOnly.TryParse(dateString, out purchaseDate);

                if (!isValidDate)
                {
                    _display.PrintColorMessage("\nInvalid date format. Please enter the date in the format yyyy-mm-dd.", ConsoleColor.Red);
                }
            }

            return purchaseDate;
        }

        private Size RegisterSize()
        {
            Size size = Size.XL;
            bool validSize = false;

            while (!validSize)
            {
                _display.PrintMessage("Size (S/M/L/XL): ");
                string sizeString = _input.GetInputFromUser().ToLower();

                switch (sizeString)
                {
                    case "s":
                        size = Size.S;
                        validSize = true;
                        break;
                    case "m":
                        size = Size.M;
                        validSize = true;
                        break;
                    case "l":
                        size = Size.L;
                        validSize = true;
                        break;
                    case "xl":
                        size = Size.XL;
                        validSize = true;
                        break;
                    default:
                        _display.PrintColorMessage("\nInvalid option. Please enter S, M, L, or XL.", ConsoleColor.Red);
                        break;
                }
            }
            return size;

        }

        private string UpdateBrandName(Garment oldGarment)
        {
            _display.PrintMessage("\nCurrent brand name is: " + oldGarment.BrandName);

            bool isValid = false;
            string newBrandName = oldGarment.BrandName;
            while (!isValid)
            {
                _display.PrintMessage("\nWould you like to change it (y / n)?");
                var answer = _input.GetInputFromUser();
                if (answer.ToLower() == "n")
                {
                    isValid = true;
                }
                if (answer.ToLower() == "y")
                {
                    newBrandName = RegisterString("Brand name");
                    isValid = true;
                }
            }
            return newBrandName;
        }

        private string UpdateColor(Garment oldGarment)
        {
            Console.WriteLine("\nCurrent color is: " + oldGarment.Color);
            bool isValid = false;
            string newColor = oldGarment.Color;
            while (!isValid)
            {
                Console.WriteLine("\nWould you like to change it (y / n)?");
                var answer = Console.ReadLine();
                if (answer.ToLower() == "n")
                {
                    isValid = true;
                }
                if (answer.ToLower() == "y")
                {
                    newColor = RegisterString("Color");
                    isValid = true;
                }
            }
            return newColor;
        }

        private DateOnly UpdatePurchase(Garment oldGarment)
        {
            Console.WriteLine("\nDate of purchase is: " + oldGarment.PurchaseDate);
            bool isValid = false;
            DateOnly newPurchase = oldGarment.PurchaseDate;
            while (!isValid)
            {
                Console.WriteLine("\nWould you like to change it (y / n)?");
                var answer = Console.ReadLine();
                if (answer.ToLower() == "n")
                {
                    isValid = true;
                }
                if (answer.ToLower() == "y")
                {
                    newPurchase = RegisterPurchaseDate();
                    isValid = true;
                }
            }
            return newPurchase;
        }
        private Size UpdateSize(Garment oldGarment)
        {
            Console.WriteLine("\nCurrest size is: " + oldGarment.Size);
            bool isValid = false;
            Size newSize = oldGarment.Size;
            while (!isValid)
            {
                Console.WriteLine("\nWould you like to change it (y / n)?");
                var answer = Console.ReadLine();
                if (answer.ToLower() == "n")
                {
                    isValid = true;
                }
                if (answer.ToLower() == "y")
                {
                    newSize = RegisterSize();
                    isValid = true;
                }
            }
            return newSize;
        }

        private Garment? GetGarment()
        {
            var idInput = _input.GetInputFromUser();
            uint id;
            Garment? searchedGarment = null;

            if (!uint.TryParse(idInput, out id))
            {
                _display.PrintColorMessage("\nInvalid ID format. Please, enter a number.", ConsoleColor.Red);
            } 
            else
            {
                searchedGarment = _garmentService.SearchGarment(id);
                if (searchedGarment == null)
                {
                    _display.PrintColorMessage("\nInvalid ID, garment not found.", ConsoleColor.Red);
                }
            }

            return searchedGarment;
        }
    }
}
