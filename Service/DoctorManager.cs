using System;
using Hospital_Test_Performance.Database;
using Hospital_Test_Performance.Models;

namespace Hospital_Test_Performance.Service
{
    public class DoctorManager
    {
        private readonly DatabaseContent _db;

        public DoctorManager(DatabaseContent db)
        {
            _db = db;
        }

        public void RegistrarDoctor()
        {
            try
            {
            Console.Write("Name: ");
            var name = Console.ReadLine();
                    DateTime dob;
                    while (true)
                    {
                        Console.Write("DOB (yyyy-MM-dd) [required]: ");
                        var dobStr = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(dobStr))
                        {
                            Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Registration cancelled: DOB is required.");
                            return;
                        }
                        if (!DateTime.TryParseExact(dobStr, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dob))
                        {
                            Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid date format. Use yyyy-MM-dd (no letters). Try again or leave empty to cancel.");
                            continue;
                        }
                        var today = DateTime.Today;
                        var age = today.Year - dob.Year;
                        if (dob.Date > today.AddYears(-age)) age--;
                        if (age < 0 || age > 120)
                        {
                            Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid age derived from DOB. Please enter a realistic birth date.");
                            continue;
                        }
                        break;
                    }
            // Phone validation: allow digits, spaces, +, -, parentheses
            string? phone;
            while (true)
            {
                Console.Write("Phone: ");
                phone = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(phone)) break; // optional
                var phoneClean = phone.Trim();
                if (System.Text.RegularExpressions.Regex.IsMatch(phoneClean, "^[0-9()+\\- ]+$")) break;
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid phone. Use only digits, spaces, +, -, and parentheses. Try again or leave empty to skip.");
            }
            Console.Write("Address: ");
            var address = Console.ReadLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();
            string? specialty;
            while (true)
            {
                Console.Write("Specialty [required]: ");
                specialty = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(specialty))
                {
                    Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Specialty is required. Please enter a specialty.");
                    continue;
                }
                break;
            }
            string? license;
            while (true)
            {
                Console.Write("License [required]: ");
                license = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(license))
                {
                    Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("License is required. Please enter a license number.");
                    continue;
                }
                break;
            }
            string? document;
            while (true)
            {
                Console.Write("Document number [required]: ");
                document = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(document))
                {
                    Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Document is required. Please enter a document number.");
                    continue;
                }
                if (_db.Doctors.Exists(x => x.DocumentNumber.Equals(document, StringComparison.OrdinalIgnoreCase)))
                {
                    Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Cannot register doctor. Document '{document}' already exists.");
                    var existing = _db.Doctors.Find(x => x.DocumentNumber.Equals(document, StringComparison.OrdinalIgnoreCase));
                    if (existing != null) Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Existing doctor: {existing.Id}: {existing.Name} - {existing.Specialty} - Doc: {existing.DocumentNumber}");
                    continue;
                }
                break;
            }

            var d = new Doctor(0, name ?? string.Empty, dob, phone ?? string.Empty, address ?? string.Empty, email ?? string.Empty, specialty ?? string.Empty, license ?? string.Empty, string.Empty)
            {
                DocumentNumber = document ?? string.Empty
            };
            d.Registrar(_db);
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error registering doctor: {ex.Message}");
            }
        }

        public void ListarDoctores(string? specialtyFilter = null)
        {
            try
            {
            var list = string.IsNullOrWhiteSpace(specialtyFilter)
                ? _db.Doctors
                : _db.Doctors.FindAll(d => !string.IsNullOrWhiteSpace(d.Specialty) && d.Specialty.Contains(specialtyFilter, StringComparison.OrdinalIgnoreCase));

            if (list.Count == 0)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("No doctors found for the given filter.");
                return;
            }

            foreach (var d in list)
            {
                Console.WriteLine($"{d.Id}: {d.Name} - {d.Specialty} - Doc: {d.DocumentNumber}");
            }
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error listing doctors: {ex.Message}");
            }
        }

        public void FindByDocument(string documentNumber)
        {
            try
            {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid document");
                return;
            }

            var d = _db.Doctors.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (d == null) Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Doctor not found");
            else Console.WriteLine($"{d.Id}: {d.Name} - {d.Specialty} - Doc: {d.DocumentNumber}");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error searching doctor: {ex.Message}");
            }
        }

        public void DeleteByDocument(string documentNumber)
        {
            try
            {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid document");
                return;
            }

            var d = _db.Doctors.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (d != null) _db.Doctors.Remove(d);
            Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess($"Doctor with document {documentNumber} deleted (if existed).");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error deleting doctor: {ex.Message}");
            }
        }

        public void UpdateByDocument(string documentNumber)
        {
            try
            {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid document");
                return;
            }

            var d = _db.Doctors.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (d == null)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Doctor not found");
                return;
            }

            Console.WriteLine($"Updating doctor {d.Name} (Doc: {d.DocumentNumber}). Leave empty to keep current value.");
            Console.Write($"Name ({d.Name}): ");
            var name = Console.ReadLine();
            Console.Write($"DOB ({d.DateOfBirth:yyyy-MM-dd}): ");
                    DateTime newDob;
                    while (true)
                    {
                        var dobInput = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(dobInput))
                        {
                            newDob = d.DateOfBirth; // keep current
                            break;
                        }
                        if (!DateTime.TryParseExact(dobInput, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out newDob))
                        {
                            Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid date format. Use yyyy-MM-dd (no letters). Try again or leave empty to keep current value.");
                            continue;
                        }
                        var today = DateTime.Today;
                        var age = today.Year - newDob.Year;
                        if (newDob.Date > today.AddYears(-age)) age--;
                        if (age < 0 || age > 120)
                        {
                            Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid age derived from DOB. Please enter a realistic birth date.");
                            continue;
                        }
                        break;
                    }
            Console.Write($"Phone ({d.Telefono}): ");
            var phone = Console.ReadLine();
            // Phone validation: allow digits, spaces, +, -, parentheses
            while (true)
            {
                if (string.IsNullOrWhiteSpace(phone)) break; // optional
                var phoneClean = phone.Trim();
                if (System.Text.RegularExpressions.Regex.IsMatch(phoneClean, "^[0-9()+\\- ]+$")) break;
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid phone. Use only digits, spaces, +, -, and parentheses. Try again or leave empty to skip.");
                phone = Console.ReadLine();
            }
            Console.Write($"Address ({d.Address}): ");
            var address = Console.ReadLine();
            Console.Write($"Email ({d.Email}): ");
            var email = Console.ReadLine();
            Console.Write($"Specialty ({d.Specialty}): ");
            var specialty = Console.ReadLine();
            Console.Write($"License ({d.LicenseNumber}): ");
            var license = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name)) d.Name = name;
            // assign validated DOB from the input loop
            d.DateOfBirth = newDob;
            if (!string.IsNullOrWhiteSpace(phone)) d.Telefono = phone;
            if (!string.IsNullOrWhiteSpace(address)) d.Address = address;
            if (!string.IsNullOrWhiteSpace(email)) d.Email = email;
            if (!string.IsNullOrWhiteSpace(specialty)) d.Specialty = specialty;
            if (!string.IsNullOrWhiteSpace(license)) d.LicenseNumber = license;

            Console.Write($"Document ({d.DocumentNumber}): ");
            var newDoc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDoc))
            {
                if (_db.Doctors.Exists(x => x.DocumentNumber.Equals(newDoc, StringComparison.OrdinalIgnoreCase) && x.Id != d.Id))
                {
                    Console.WriteLine($"Cannot change document. Document '{newDoc}' already exists.");
                    var existing = _db.Doctors.Find(x => x.DocumentNumber.Equals(newDoc, StringComparison.OrdinalIgnoreCase));
                    if (existing != null) Console.WriteLine($"Existing doctor: {existing.Id}: {existing.Name} - {existing.Specialty} - Doc: {existing.DocumentNumber}");
                }
                else
                {
                    d.DocumentNumber = newDoc;
                }
            }

            Console.WriteLine("Doctor updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating doctor: {ex.Message}");
            }
        }
    }
}