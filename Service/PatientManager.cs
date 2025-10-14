using System;
using System.Collections.Generic;
using Hospital_Test_Performance.Database;
using Hospital_Test_Performance.Models;

namespace Hospital_Test_Performance.Service
{
    public class PatientManager
    {
        private readonly DatabaseContent _db;

        public PatientManager(DatabaseContent db)
        {
            _db = db;
        }

        public void RegistrarCliente()
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
                // uniqueness check
                if (_db.Patients.Exists(x => x.DocumentNumber.Equals(document, StringComparison.OrdinalIgnoreCase)))
                {
                    Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Cannot register patient. Document '{document}' already exists.");
                    var existing = _db.Patients.Find(x => x.DocumentNumber.Equals(document, StringComparison.OrdinalIgnoreCase));
                    if (existing != null)
                    {
                        Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Existing patient: {existing.Id}: {existing.Name} - {existing.Email} - Doc: {existing.DocumentNumber}");
                    }
                    // ask again
                    continue;
                }
                break;
            }

            var p = new Patient { Name = name ?? string.Empty, DateOfBirth = dob, Telefono = phone ?? string.Empty, Address = address ?? string.Empty, Email = email ?? string.Empty, DocumentNumber = document ?? string.Empty };
            p.Registrar(_db);
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error registering patient: {ex.Message}");
            }
        }

        public void ListarClientes()
        {
            try
            {
            foreach (var p in _db.Patients)
            {
                Console.WriteLine($"{p.Id}: {p.Name} - {p.Email} - Doc: {p.DocumentNumber}");
            }
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error listing patients: {ex.Message}");
            }
        }
        public void FindByDocument(string documentNumber)
        {
            try
            {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Documento inválido");
                return;
            }

            var p = _db.Patients.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (p == null) Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Patient not found");
            else Console.WriteLine($"{p.Id}: {p.Name} - {p.Email} - Doc: {p.DocumentNumber}");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error searching patient: {ex.Message}");
            }
        }

        public void DeleteByDocument(string documentNumber)
        {
            try
            {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Documento inválido");
                return;
            }

            var p = _db.Patients.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (p != null) _db.Patients.Remove(p);
            Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess($"Patient with document {documentNumber} deleted (if existed).");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error deleting patient: {ex.Message}");
            }
        }

        public void UpdateByDocument(string documentNumber)
        {
            try
            {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Documento inválido");
                return;
            }

            var p = _db.Patients.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (p == null)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Patient not found");
                return;
            }

            Console.WriteLine($"Updating patient {p.Name} (Doc: {p.DocumentNumber}). Leave empty to keep current value.");
            Console.Write($"Name ({p.Name}): ");
            var name = Console.ReadLine();
            Console.Write($"DOB ({p.DateOfBirth:yyyy-MM-dd}): ");
                    DateTime newDob;
                    while (true)
                    {
                        var dobInput = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(dobInput))
                        {
                            newDob = p.DateOfBirth; // Keep current value if empty
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
            Console.Write($"Phone ({p.Telefono}): ");
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
            Console.Write($"Address ({p.Address}): ");
            var address = Console.ReadLine();
            Console.Write($"Email ({p.Email}): ");
            var email = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name)) p.Name = name;
            Console.Write($"Document ({p.DocumentNumber}): ");
            var newDoc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDoc))
            {
                // check uniqueness
                if (_db.Patients.Exists(x => x.DocumentNumber.Equals(newDoc, StringComparison.OrdinalIgnoreCase) && x.Id != p.Id))
                {
                    Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Cannot change document. Document '{newDoc}' already exists.");
                    var existing = _db.Patients.Find(x => x.DocumentNumber.Equals(newDoc, StringComparison.OrdinalIgnoreCase));
                    if (existing != null) Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Existing patient: {existing.Id}: {existing.Name} - {existing.Email} - Doc: {existing.DocumentNumber}");
                }
                else
                {
                    p.DocumentNumber = newDoc;
                }
            }
                    p.DateOfBirth = newDob; // Update with validated DOB
            if (!string.IsNullOrWhiteSpace(phone)) p.Telefono = phone;
            if (!string.IsNullOrWhiteSpace(address)) p.Address = address;
            if (!string.IsNullOrWhiteSpace(email)) p.Email = email;

            Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess("Patient updated.");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error updating patient: {ex.Message}");
            }
        }
    }
}