(The file `/home/Coder/Escritorio/Hospital-Test-Performance/Readme.md` exists, but is empty)
# Hospital Test Performance

Lightweight console application to manage a small in-memory hospital test dataset.

This repository demonstrates a minimal layered architecture (models, repositories, services/managers, and console UI) and includes:

- Patients and Doctors models (inherit from `Person`)
- Appointment scheduling with conflict checks
- Email confirmation flow with in-memory email history
- Simple repositories that wrap an in-memory `DatabaseContent`
- Small utilities (colored console helper, menu screens)

---

## Table of contents

- Project structure
- How to run
- Email / SMTP setup (send real emails)
- Class diagram generator (PlantUML) usage
- Developer notes
- Screenshot (placeholder)

---

## Project structure

Top-level files and directories (important ones):

```
Hospital-Test-Performance.sln
Hospital-Test-Performance.csproj
Program.cs                 # App entrypoint and dependency wiring
Readme.md                  # (this file)
Database/                  # In-memory data store (seeded)
	DatabaseContent.cs
Interface/                 # Interfaces (IRepository, IRegistable, IAttendable)
Models/                    # Domain models (Person, Patient, Doctor, Appointment, EmailRecord)
Repository/                # Repositories (PatientRepository, DoctorRepository)
Service/                   # Business services/managers (PatientManager, DoctorManager, AppointmentManager, EmailService)
Utils/                     # Console helpers and menus
Tools/                     # Optional tools (e.g., ClassDiagramGenerator)
```

Key classes:

- `Models.Person` — base class for `Patient` and `Doctor` (has DocumentNumber)
- `Models.Patient`, `Models.Doctor` — domain models
- `Models.Appointment` — appointment detail + status
- `Repository.*Repository` — CRUD operations for in-memory lists
- `Service.*Manager` — validation and interactive console flows
- `Service.EmailService` — sends emails via SMTP and records attempts in `DatabaseContent.EmailHistory`

---

## How to run

Prerequisites:

- .NET 8 SDK installed
- Network access to SMTP server if you want real email sending

Build and run:

```bash
dotnet build
dotnet run
```

The console menu is interactive. Main features:

- Patient management (register, list, find, update, delete)
- Doctor management (register, list, find, update, delete)
- Appointment management (schedule, list, change status, view email history, retry email)

---

## Email / SMTP setup (send real emails)

The app uses environment variables for SMTP configuration. Recommended for Gmail:

1. Enable 2-Step Verification in your Google account.
2. Create an App Password for Mail, save it.

Set these variables in your shell before running the app:

```bash
export SMTP_HOST="smtp.gmail.com"
export SMTP_PORT="587"
export SMTP_USER="your@gmail.com"
export SMTP_PASS="your_app_password"
```

Optional: to redirect all outgoing messages to a single test address (useful while developing):

```bash
export EMAIL_OVERRIDE_RECIPIENT="your-test-address@example.com"
```

Notes:

- The app will record every attempted send in `DatabaseContent.EmailHistory`. If a send fails, `ErrorDetail` contains the exception text to diagnose the failure.
- If you use Gmail, prefer App Passwords or OAuth (App Passwords are easiest for testing).

---

## Class diagram generator (PlantUML)

There's a small tool under `Tools/ClassDiagramGenerator` that scans `.cs` files and emits a PlantUML file (simple extraction of classes, fields, properties, and inheritance).

Usage example (from repo root):

```bash
dotnet run --project Tools/ClassDiagramGenerator -- . diagram.puml
```

Then open `diagram.puml` with PlantUML extension or render using PlantUML to PNG/SVG.

If you prefer, I can run the generator and add the generated `diagram.puml` to the repo.

---

## Developer notes

- The app uses a thin UI model: menus in `Utils/` call managers in `Service/`. Managers perform validation and call repositories for persistence.
- Persistence is in-memory (`DatabaseContent`) and is not persisted to disk — suitable for demos and tests.
- Email sending uses `System.Net.Mail.SmtpClient` (synchronous). You can replace `EmailService` with an async client or a third-party provider SDK.

Suggested improvements:

- Add unit tests for managers and repositories
- Replace in-memory store with a lightweight database (SQLite or real DB) and update repositories
- Add better configuration management (appsettings.json) and secrets handling

---

## Screenshot

Paste a screenshot of your running application here (or a path to an image):

![App screenshot](/Docs/Captura%20desde%202025-10-14%2019-32-03.png)
![App screenshot](/Docs/Captura%20desde%202025-10-14%2019-32-07.png)
![App screenshot](/Docs/Captura%20desde%202025-10-14%2019-32-18.png)
Replace the image or the path with a real capture when you have it.

---

If you want, I can also:

- Add a `docs/` folder with the generated diagram and a screenshot if you provide one.
- Add a small shell script to set up environment variables for SMTP quickly.

If you want any change in the README (tone, extra sections, commands for Windows PowerShell), tell me which and I update it.
