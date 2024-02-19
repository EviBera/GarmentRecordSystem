using GarmentRecordSystem.Models;
using GarmentRecordSystem.Services;

namespace ConsoleBasedUI.Services
{
    public class HandleMainMenuOptions : IHandleMainMenu
    {
        private IJsonHandlerService _jsonHandlerService;
        private IGarmentService _garmentService;

        public HandleMainMenuOptions()
        {
            _jsonHandlerService = new JsonHandlerService();
            _garmentService = new GarmentService(_jsonHandlerService);
        }


        public void AddGarment()
        {
            Console.WriteLine("\nTo add a new garment you have to specify the following (please, type in):\n");
            var brandName = RegisterString("Brand name");
            var purchaseDate = RegisterPurchaseDate();
            var color = RegisterString("Color");
            var size = RegisterSize();

            try
            {
                _garmentService.AddGarment(new Garment(brandName, purchaseDate, color, size));
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nGarment added successfully. (Not saved into file yet.)");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ShowMeWhatHappened();
        }

        public void UpdateGarment()
        {
            Console.Write("\nEnter the ID of the garment that needs an update: ");
            var idInput = Console.ReadLine();
            uint id;

            if (!uint.TryParse(idInput, out id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid ID format.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            Garment? oldGarment = _garmentService.SearchGarment(id);
            if (oldGarment == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid ID, garment not found.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            var newBrandName = UpdateBrandName(oldGarment);
            var newColor = UpdateColor(oldGarment);
            var newPurchase = UpdatePurchase(oldGarment);
            var newSize = UpdateSize(oldGarment);

            try
            {
                _garmentService.UpdateGarment(id, newBrandName, newPurchase, newColor, newSize);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nGarment udated successfully. (Not saved into file yet.)");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ShowMeWhatHappened();
        }

        public void DeleteGarment()
        {
            Console.Write("\nWhich garment would you like to delete? Enter ID: ");
            var idInput = Console.ReadLine();
            uint id;

            if (!uint.TryParse(idInput, out id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid ID format.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            Garment? garment = _garmentService.SearchGarment(id);
            if (garment == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid ID, garment not found.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            try
            {
                _garmentService.DeleteGarment(id);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nGarment deleted successfully. (Changes are not saved into file yet.)");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ShowMeWhatHappened();
        }

        public void SearchGarment()
        {
            Console.Write("\nWhich garment would you like to see? Enter ID: ");
            var idInput = Console.ReadLine();
            uint id;

            if (!uint.TryParse(idInput, out id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid ID format.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            Garment? garment = _garmentService.SearchGarment(id);
            if (garment == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid ID, garment not found.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            else
            {
                Console.WriteLine("\nThe garment you are looking for:");
                Console.WriteLine(garment.ToString());
            }

        }

        public void SortGarments()
        {
            List<Garment> sortedList = new();

            Console.WriteLine("\nYou can select which factor use for sorting.");
            Console.WriteLine("1. Sort by brand name");
            Console.WriteLine("2. Sort by date of purchase");
            Console.WriteLine("3. Sort by size");
            Console.WriteLine("4. Sort by color");
            Console.Write("Enter your choice: ");

            string answer = Console.ReadLine();
            switch (answer)
            {
                case "1":
                    sortedList = _garmentService.SortGarments(SortGarmentsCriteria.BrandName);
                    break;
                case "2":
                    sortedList = _garmentService.SortGarments(SortGarmentsCriteria.PurchaseDate);
                    break;
                case "3":
                    sortedList = _garmentService.SortGarments(SortGarmentsCriteria.Size);
                    break;
                case "4":
                    sortedList = _garmentService.SortGarments(SortGarmentsCriteria.Color);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid option. Please select a valid number (1 - 4).");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.WriteLine("\nThe garments sorted by your choice: \n");
            foreach (Garment g in sortedList)
            {
                Console.WriteLine(g.ToString());
            }
        }

        public void SaveGarmentsToFile()
        {
            bool isValid = false;
            string path = String.Empty;
            while (!isValid)
            {
                Console.Write("\nEnter file path for saving data: ");
                path = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(path))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The file path cannot be empty. Please enter a valid path.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The file path contains invalid characters. Please enter a valid path.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (!path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The file must be a .json file. Please enter a valid file path ending with '.json'.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    isValid = true;
                }

            }

            try
            {
                _jsonHandlerService.SaveToFile(path, _garmentService.GetAllGarments());
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("File saved successfully.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void LoadGarmentsFromFile()
        {
            bool isValid = false;
            string path = String.Empty;
            while (!isValid)
            {
                Console.Write("\nEnter required file path: ");
                path = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(path))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The file path cannot be empty. Please enter a valid path.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The file path contains invalid characters. Please enter a valid path.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (!File.Exists(path))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The file does not exist. Please enter a valid path to an existing file.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (!path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The file must be a .json file. Please enter a valid file path ending with '.json'.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    isValid = true;
                }

            }

            try
            {
                _garmentService.LoadGarments(path);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("File loaded successfully.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void DisplayTheWholeCollection()
        {
            Console.WriteLine("\nIn-memory garments: \n");
            foreach (var garment in _garmentService.GetAllGarments())
            {
                Console.WriteLine(garment.ToString());
            }

        }

        string RegisterString(string arg)
        {
            string data = string.Empty;
            bool isValid = false;

            while (!isValid)
            {
                Console.WriteLine(arg + ": ");
                data = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(data))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(arg + " cannot be empty. Please enter valid data.");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                else if (data.Length > 50)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(arg + " cannot exceed 50 characters. Please enter a shorter text.");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                else
                {
                    int specialCharCount = data.Count(c => Path.GetInvalidFileNameChars().Contains(c));
                    if (specialCharCount > 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(arg + " can contain at most one special character. Please revise your input.");
                        Console.ForegroundColor = ConsoleColor.White;
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

        DateOnly RegisterPurchaseDate()
        {
            DateOnly purchaseDate = default;
            bool isValidDate = false;

            while (!isValidDate)
            {
                Console.WriteLine("Purchase date (yyyy-mm-dd): ");
                var dateString = Console.ReadLine();

                isValidDate = DateOnly.TryParse(dateString, out purchaseDate);

                if (!isValidDate)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid date format. Please enter the date in the format yyyy-mm-dd.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            return purchaseDate;
        }

        Size RegisterSize()
        {
            Size size = Size.XL;
            bool validSize = false;

            while (!validSize)
            {
                Console.WriteLine("Size (S/M/L/XL): ");
                var sizeString = Console.ReadLine().ToLower();

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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInvalid option. Please enter S, M, L, or XL.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
            return size;

        }

        void ShowMeWhatHappened()
        {
            var garments = _garmentService.GetAllGarments();
            foreach (var garment in garments)
            {
                Console.WriteLine(garment.ToString());
            }

        }

        string UpdateBrandName(Garment oldGarment)
        {
            Console.WriteLine("\nCurrent brand name is: " + oldGarment.BrandName);
            bool isValid = false;
            string newBrandName = oldGarment.BrandName;
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
                    newBrandName = RegisterString("Brand name");
                    isValid = true;
                }
            }
            return newBrandName;
        }
        string UpdateColor(Garment oldGarment)
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
        DateOnly UpdatePurchase(Garment oldGarment)
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
        Size UpdateSize(Garment oldGarment)
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

        public void Exit(ref bool exit)
        {
            exit = true;
        }
    }
}
