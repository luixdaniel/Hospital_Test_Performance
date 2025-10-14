using System;
using Hospital_Test_Performance.Database;
using Hospital_Test_Performance.Models;

namespace Hospital_Test_Performance.Service
{
    /// <summary>
    /// Service class that handles doctor-related user flows and validations.
    /// Uses a repository for persistence operations.
    /// </summary>
    public class DoctorManager
    {
        private readonly Hospital_Test_Performance.Interface.IRepository<Doctor> _repo;

        public DoctorManager(Hospital_Test_Performance.Interface.IRepository<Doctor> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

    /// <summary>Interactively register a new doctor (reads from Console, validates and persists).</summary>
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
                if (_repo.GetByDocument(document) != null)
                {
                    Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Cannot register doctor. Document '{document}' already exists.");
                    var existing = _repo.GetByDocument(document);
                    if (existing != null) Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Existing doctor: {existing.Id}: {existing.Name} - {existing.Specialty} - Doc: {existing.DocumentNumber}");
                    continue;
                }
                break;
            }

            var d = new Doctor(0, name ?? string.Empty, dob, phone ?? string.Empty, address ?? string.Empty, email ?? string.Empty, specialty ?? string.Empty, license ?? string.Empty, string.Empty)
            {
                DocumentNumber = document ?? string.Empty
            };
            _repo.Add(d);
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error registering doctor: {ex.Message}");
            }
        }

    /// <summary>List doctors, optionally filtered by specialty.</summary>
    /// <param name="specialtyFilter">Optional substring to filter specialty.</param>
    public void ListarDoctores(string? specialtyFilter = null)
    {
            try
            {
            var all = _repo.GetAll();
            var list = string.IsNullOrWhiteSpace(specialtyFilter)
                ? all
                : all.Where(d => !string.IsNullOrWhiteSpace(d.Specialty) && d.Specialty.Contains(specialtyFilter, StringComparison.OrdinalIgnoreCase));

            if (!list.Any())
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

    /// <summary>Find a doctor by document number and print details to the console.</summary>
    /// <param name="documentNumber">Document number to search for.</param>
    public void FindByDocument(string documentNumber)
    {
            try
            {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid document");
                return;
            }

            var d = _repo.GetByDocument(documentNumber);
            if (d == null) Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Doctor not found");
            else Console.WriteLine($"{d.Id}: {d.Name} - {d.Specialty} - Doc: {d.DocumentNumber}");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error searching doctor: {ex.Message}");
            }
        }

    /// <summary>Delete a doctor by document number.</summary>
    /// <param name="documentNumber">Document number of the doctor to delete.</param>
    public void DeleteByDocument(string documentNumber)
    {
            try
            {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid document");
                return;
            }

            var removed = _repo.DeleteByDocument(documentNumber);
            if (removed)
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess($"Doctor with document {documentNumber} deleted.");
            else
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Doctor with document {documentNumber} not found.");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error deleting doctor: {ex.Message}");
            }
        }

    /// <summary>Update a doctor's details by their document number (interactive).</summary>
    /// <param name="documentNumber">Document number of the doctor to update.</param>
    public void UpdateByDocument(string documentNumber)
    {
            try
            {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid document");
                return;
            }

            var d = _repo.GetByDocument(documentNumber);
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
                if (_repo.GetByDocument(newDoc) != null && _repo.GetByDocument(newDoc)?.Id != d.Id)
                {
                    Console.WriteLine($"Cannot change document. Document '{newDoc}' already exists.");
                    var existing = _repo.GetByDocument(newDoc);
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