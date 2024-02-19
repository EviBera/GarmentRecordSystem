using ConsoleBasedUI.Services;
using ConsoleBasedUI.UI;

Display display = new();
Input input = new();
HandleMainMenuOptions handleMainMenu = new();

HandleMainMenu();


void HandleMainMenu()
{
    bool exit = false;
    while (!exit)
    {
        display.PrintMessage("\nGarment Record System:");
        display.PrintMessage("1. Add garment");
        display.PrintMessage("2. Update garment");
        display.PrintMessage("3. Delete garment");
        display.PrintMessage("4. Search for garment");
        display.PrintMessage("5. Sort garments");
        display.PrintMessage("6. Save garments to file");
        display.PrintMessage("7. Load garments from file");
        display.PrintMessage("8. Display file content");
        display.PrintMessage("9. Exit");
        display.PrintMessage("Select an option: ");

        string? usersChoice = input.GetInputFromUser();
        int option;
        bool isValidInput = int.TryParse(usersChoice, out option);

        if (!isValidInput)
        {
            display.PrintColorMessage("\nInvalid option. Enter a number.", ConsoleColor.Red);
            continue;
        }

        switch (option)
        {
            case 1:
                handleMainMenu.AddGarment();
                break;
            case 2:
                handleMainMenu.UpdateGarment();
                break;
            case 3:
                handleMainMenu.DeleteGarment();
                break;
            case 4:
                handleMainMenu.SearchGarment();
                break;
            case 5:
                handleMainMenu.SortGarments();
                break;
            case 6:
                handleMainMenu.SaveGarmentsToFile();
                break;
            case 7:
                handleMainMenu.LoadGarmentsFromFile();
                break;
            case 8:
                handleMainMenu.DisplayTheWholeCollection();
                break;
            case 9:
                handleMainMenu.Exit(ref exit);
                break;
        
            default:
                display.PrintColorMessage("\nInvalid option. Please try again.", ConsoleColor.Red);
                break;
        }
    }
}
