using Hospital_Test_Performance.Utils;
using Hospital_Test_Performance.Database;
using Hospital_Test_Performance.Service;

var db = new DatabaseContent();
var patientManager = new Hospital_Test_Performance.Service.PatientManager(db);
var doctorManager = new Hospital_Test_Performance.Service.DoctorManager(db);
var appointmentManager = new Hospital_Test_Performance.Service.AppointmentManager(db);

bool exit = false;
while (!exit)
{
    ShowMenu.DisplayMenu();
    var choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            try
            {
                ShowMenuPatient.DisplayMenuPatient(patientManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in patient menu: {ex.Message}");
            }
            break;
        case "2":
            try
            {
                ShowMenuDoctor.DisplayMenuDoctor(doctorManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in doctor menu: {ex.Message}");
            }
            break;
        case "3":
            try
            {
                ShowMenuAppointment.DisplayMenuAppointment(appointmentManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in appointment menu: {ex.Message}");
            }
            break;
        case "4":
            exit = true;
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}