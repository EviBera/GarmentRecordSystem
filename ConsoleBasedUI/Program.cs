using GarmentRecordSystem;
using GarmentRecordSystem.Services;
using GarmentRecordSystem.Models;

IGarmentService garmentService = new GarmentService();
IJsonHandlerService jsonHandlerService = new JsonHandlerService();
string filePath = "garments.json";

bool exit = false;
while (!exit)
{
    Console.WriteLine("\nGarment Record System:");
    Console.WriteLine("1. Add garment");
    Console.WriteLine("2. Update garment");
    Console.WriteLine("3. Delete garment");
    Console.WriteLine("4. Search for garment");
    Console.WriteLine("5. Sort garments");
    Console.WriteLine("6. Save garments to file");
    Console.WriteLine("7. Load garments from file");
    Console.WriteLine("8. Display file content");
    Console.WriteLine("9. Exit");
    Console.Write("Select an option: ");

    string input = Console.ReadLine();
    int option;
    bool isValidInput = int.TryParse(input, out option);

    if (!isValidInput)
    {
        Console.WriteLine("Invalid option. Enter a number.");
        continue;
    }

    switch (option)
    {
        case 1:
            AddGarment();
            break;
        case 2:
            UpdateGarment();
            break;
        case 3:
            DeleteGarment();
            break;
        case 4:
            SearchGarment();
            break;
        case 5:
            SortGarments();
            break;
        case 6:
            SaveGarmentsToFile();
            break;
        case 7:
            LoadGarmentsFromFile();
            break;
        case 8:
            DisplayTheWholeCollection();
            break;
        case 9:
            exit = true;
            break;
        
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}

void AddGarment()
{
    Console.WriteLine("\nTo add a new garment you have to specify the following (please, type in):\n");
    var brandName = RegisterString("Brand name");
    var purchaseDate = RegisterPurchaseDate();
    var color = RegisterString("Color");
    var size = RegisterSize();

    try
    {
        garmentService.AddGarment(new Garment(brandName, purchaseDate, color, size));
        Console.WriteLine("Garment added successfully. (Not saved into file yet.)");
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }

    var garments = garmentService.GetAllGarments();
    foreach (var garment in garments)
    {
        Console.WriteLine(garment.BrandName + garment.GarmentID);
    }
    
}

static void UpdateGarment()
{
    Console.WriteLine("UpdateGarment method");
}

static void DeleteGarment()
{
    Console.WriteLine("DeleteGarment method");
}

static void SearchGarment()
{
    Console.WriteLine("SearchGarment method");
}

static void SortGarments()
{
    Console.WriteLine("SortGarment method");
}

static void SaveGarmentsToFile()
{
    Console.WriteLine("Garments saved to file successfully.");
}

static void LoadGarmentsFromFile()
{
    Console.WriteLine("Garments loaded from file successfully.");
}

void DisplayTheWholeCollection()
{
    Console.WriteLine("\nThe json file includes these garments: \n");

    var garments = jsonHandlerService.LoadFromFile(filePath);
    foreach (var garment in garments)
    {
        Console.WriteLine("ID: " + garment.GarmentID);
        Console.WriteLine("Brand: " + garment.BrandName);
        Console.WriteLine("Date of purchase: " + garment.PurchaseDate);
        Console.WriteLine("Color: " + garment.Color);
        Console.WriteLine("Size: " + garment.Size + "\n");

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
            Console.WriteLine(arg + " cannot be empty. Please enter valid data.");
        }

        else if (data.Length > 50)
        {
            Console.WriteLine(arg + " cannot exceed 50 characters. Please enter a shorter text.");
        }

        else
        {
            int specialCharCount = data.Count(c => Path.GetInvalidFileNameChars().Contains(c));
            if (specialCharCount > 1)
            {
                Console.WriteLine(arg + " can contain at most one special character. Please revise your input.");
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
            Console.WriteLine("Invalid date format. Please enter the date in the format yyyy-mm-dd.");
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
                Console.WriteLine("Invalid option. Please enter S, M, L, or XL.");
                break;
        }
    }
    return size;

}
