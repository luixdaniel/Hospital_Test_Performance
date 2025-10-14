using System;
using System.Collections.Generic;
using Hospital_Test_Performance.Database;
using Hospital_Test_Performance.Models;

namespace Hospital_Test_Performance.Service
{
    /// <summary>
    /// Service class that handles patient-related user flows and validations.
    /// Uses a repository for persistence operations.
    /// </summary>
    public class PatientManager
    {
        private readonly Hospital_Test_Performance.Interface.IRepository<Patient> _repo;

        public PatientManager(Hospital_Test_Performance.Interface.IRepository<Patient> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

    /// <summary>Interactively register a new patient (reads from Console, validates and persists).</summary>
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
                // uniqueness check via repo
                if (_repo.GetByDocument(document) != null)
                {
                    Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Cannot register patient. Document '{document}' already exists.");
                    var existing = _repo.GetByDocument(document);
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
            // persist via repository
            _repo.Add(p);
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error registering patient: {ex.Message}");
            }
        }

    /// <summary>List all patients to the console.</summary>
    public void ListarClientes()
    {
            try
            {
            foreach (var p in _repo.GetAll())
            {
                Console.WriteLine($"{p.Id}: {p.Name} - {p.Email} - Doc: {p.DocumentNumber}");
            }
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error listing patients: {ex.Message}");
            }
        }
    /// <summary>Find a patient by document number and print details to the console.</summary>
    /// <param name="documentNumber">Document number to search for.</param>
    public void FindByDocument(string documentNumber)
    {
            try
            {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Documento inválido");
                return;
            }

            var p = _repo.GetByDocument(documentNumber);
            if (p == null) Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Patient not found");
            else Console.WriteLine($"{p.Id}: {p.Name} - {p.Email} - Doc: {p.DocumentNumber}");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error searching patient: {ex.Message}");
            }
        }

    /// <summary>Delete a patient identified by document number.</summary>
    /// <param name="documentNumber">Document number of the patient to delete.</param>
    public void DeleteByDocument(string documentNumber)
    {
            try
            {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Documento inválido");
                return;
            }

            var removed = _repo.DeleteByDocument(documentNumber);
            if (removed)
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess($"Patient with document {documentNumber} deleted.");
            else
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Patient with document {documentNumber} not found.");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error deleting patient: {ex.Message}");
            }
        }

    /// <summary>Update a patient's details by their document number (interactive).</summary>
    /// <param name="documentNumber">Document number of the patient to update.</param>
    public void UpdateByDocument(string documentNumber)
    {
            try
            {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Documento inválido");
                return;
            }

            var p = _repo.GetByDocument(documentNumber);
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
                if (_repo.GetByDocument(newDoc) != null && _repo.GetByDocument(newDoc)?.Id != p.Id)
                {
                    Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Cannot change document. Document '{newDoc}' already exists.");
                    var existing = _repo.GetByDocument(newDoc);
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

            // persist changes
            _repo.Update(p);
            Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess("Patient updated.");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error updating patient: {ex.Message}");
            }
        }
    }
}