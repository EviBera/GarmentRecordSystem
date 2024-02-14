﻿using GarmentRecordSystem;
using GarmentRecordSystem.Services;
using GarmentRecordSystem.Models;

IGarmentService garmentService = new GarmentService();
IJsonHandlerService jsonHandlerService = new JsonHandlerService();
string filePath = "garments.json";

HandleUI();


void HandleUI()
{
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
            Console.WriteLine("\nInvalid option. Enter a number.");
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
                Console.WriteLine("\nInvalid option. Please try again.");
                break;
        }
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
        Console.WriteLine("\nGarment added successfully. (Not saved into file yet.)");
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }

    ShowMeWhatHappened();
}

void UpdateGarment()
{
    Console.Write("\nEnter the ID of the garment that needs an update: ");
    var idInput = Console.ReadLine();
    uint id;

    if (!uint.TryParse(idInput, out id))
    {
        Console.WriteLine("\nInvalid ID format.");
        return;
    }
    Garment? oldGarment = garmentService.SearchGarment(id);
    if(oldGarment == null)
    {
        Console.WriteLine("\nInvalid ID, garment not found.");
        return;
    }

    var newBrandName = UpdateBrandName(oldGarment);
    var newColor = UpdateColor(oldGarment);
    var newPurchase = UpdatePurchase(oldGarment);
    var newSize = UpdateSize(oldGarment);

    try
    {
        garmentService.UpdateGarment(id, newBrandName, newPurchase, newColor, newSize);
        Console.WriteLine("\nGarment udated successfully. (Not saved into file yet.)");
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }

    ShowMeWhatHappened();
}

void DeleteGarment()
{
    Console.Write("\nWhich garment would you like to delete? Enter ID: ");
    var idInput = Console.ReadLine();
    uint id;

    if (!uint.TryParse(idInput, out id))
    {
        Console.WriteLine("\nInvalid ID format.");
        return;
    }
    Garment? garment = garmentService.SearchGarment(id);
    if (garment == null)
    {
        Console.WriteLine("\nInvalid ID, garment not found.");
        return;
    }

    try
    {
        garmentService.DeleteGarment(id);
        Console.WriteLine("\nGarment deleted successfully. (Changes are not saved into file yet.)");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }

    ShowMeWhatHappened();
}

void SearchGarment()
{
    Console.Write("\nWhich garment would you like to see? Enter ID: ");
    var idInput = Console.ReadLine();
    uint id;

    if (!uint.TryParse(idInput, out id))
    {
        Console.WriteLine("\nInvalid ID format.");
        return;
    }
    Garment? garment = garmentService.SearchGarment(id);
    if (garment == null)
    {
        Console.WriteLine("\nInvalid ID, garment not found.");
        return;
    } else
    {
        Console.WriteLine("\nThe garment you are looking for:");
        Console.WriteLine(garment.ToString());
    }

}

void SortGarments()
{
    List<Garment> sortedList = new();

    Console.WriteLine("\nYou can select which factor use for sorting.");
    Console.WriteLine("1. Sort by brand name");
    Console.WriteLine("2. Sort by date of purchase");
    Console.WriteLine("3. Sort by size");
    Console.WriteLine("4. Sort by color");
    Console.Write("Enter your choice: ");

    string answer = Console.ReadLine();
    switch(answer)
    {
        case "1":
            sortedList = garmentService.SortGarments(SortGarmentsCriteria.BrandName);
            break;
        case "2":
            sortedList = garmentService.SortGarments(SortGarmentsCriteria.PurchaseDate);
            break;
        case "3":
            sortedList = garmentService.SortGarments(SortGarmentsCriteria.Size);
            break;
        case "4":
            sortedList = garmentService.SortGarments(SortGarmentsCriteria.Color);
            break;
        default:
            Console.WriteLine("\nInvalid option. Please select a valid number (1 - 4).");
            break;
    }

    Console.WriteLine("\nThe garments sorted by your choice: \n");
    foreach (Garment g in sortedList)
    {
        Console.WriteLine(g.ToString());
    }
}

void SaveGarmentsToFile()
{
    Console.WriteLine("Garments saved to file successfully.");
}

void LoadGarmentsFromFile()
{
    Console.WriteLine("Garments loaded from file successfully.");
}

void DisplayTheWholeCollection()
{
    Console.WriteLine("\nThe json file includes these garments: \n");

    var garments = jsonHandlerService.LoadFromFile(filePath);
    foreach (var garment in garments)
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
            Console.WriteLine("\nInvalid date format. Please enter the date in the format yyyy-mm-dd.");
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
                Console.WriteLine("\nInvalid option. Please enter S, M, L, or XL.");
                break;
        }
    }
    return size;

}

void ShowMeWhatHappened()
{
    var garments = garmentService.GetAllGarments();
    foreach (var garment in garments)
    {
        Console.WriteLine(garment.ToString());
    }

}

string UpdateBrandName(Garment oldGarment){
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
string UpdateColor(Garment oldGarment){
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
DateOnly UpdatePurchase(Garment oldGarment){
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
Size UpdateSize(Garment oldGarment){
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
