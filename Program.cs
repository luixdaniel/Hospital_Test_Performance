using Hospital_Test_Performance.Utils;
using Hospital_Test_Performance.Database;
using Hospital_Test_Performance.Service;

var db = new DatabaseContent();
var patientManager = new Hospital_Test_Performance.Service.PatientManager(db);
var doctorManager = new Hospital_Test_Performance.Service.DoctorManager(db);

bool exit = false;
while (!exit)
{
    ShowMenu.DisplayMenu();
    var choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            ShowMenuPatient.DisplayMenuPatient(patientManager);
            break;
        case "2":
            ShowMenuDoctor.DisplayMenuDoctor(doctorManager);
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