using GarmentRecordSystem;
using GarmentRecordSystem.Services; 

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

static void AddGarment()
{
    Console.WriteLine("AddGarment method");
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
    Console.WriteLine("The json file includes these garments: ");
}
