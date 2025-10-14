using Hospital_Test_Performance.Utils;

bool exit = false;
while (!exit)
{
    ShowMenu.DisplayMenu();
    var choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            ShowMenuPatient.DisplayMenuPatient();
            break;
        case "2":
            ShowMenuDoctor.DisplayMenuDoctor();
            break;
        case "3":
            ShowMenuAppointment.DisplayMenuAppointment();
            break;
        case "4":
            exit = true;
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}