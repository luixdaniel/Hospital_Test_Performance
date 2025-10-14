Proyecto: Hospital-Test-Performance
===============================

Resumen
-------
Proyecto de ejemplo en C#/.NET 8 que simula una pequeña aplicación de consola para gestionar personas en un hospital (pacientes y doctores). Usa una base en memoria (`DatabaseContent`) y menús sencillos en consola. Está pensado como base para pruebas y aprendizaje.

Estructura clave
----------------
- `Program.cs` — Punto de entrada; crea la instancia de `DatabaseContent`, managers y muestra los menús.
- `Models/Person.cs` — Clase base con propiedades comunes (Id, Name, DateOfBirth, Phone, Address, Email, Age).
- `Models/Patient.cs` — Hereda de `Person`; propiedades médicas y método `Registrar(DatabaseContent db)`.
- `Models/Doctor.cs` — Hereda de `Person`; propiedades profesionales y `Registrar(DatabaseContent db)`.
- `Interface/IRegistable.cs` — Interfaz con el contrato `void Registrar(DatabaseContent db)` para entidades registrables.
- `Database/DatabaseContent.cs` — Contenedor en memoria (`List<Patient> Patients`, `List<Doctor> Doctors`) y datos semilla.
- `Service/PatientManager.cs`, `Service/DoctorManager.cs` — Lógica de negocio mínima (registrar, listar, buscar, eliminar).
- `Utils/ShowMenu*.cs` — Utilidades de menú; la intención es que sean "thin UIs" que solo llamen a métodos de los managers.

Cómo ejecutar
-------------
Desde la raíz del proyecto:

```bash
dotnet build
dotnet run --project Hospital-Test-Performance.csproj
```

Si quieres ejecutar con entrada redirigida para pruebas automatizadas, ten en cuenta que los menús comprueban `Console.IsInputRedirected` y evitan `Console.ReadKey()` para que no bloqueen.

Contrato `IRegistable` y uso rápido
---------------------------------
La interfaz `IRegistable` define:

- `void Registrar(DatabaseContent db)` — Implementación que debe añadir la entidad correspondiente a la colección del `DatabaseContent` y asignar un id único.

Ejemplo mínimo (ya implementado en `Patient`):

- `Registrar` debe calcular un `Id` (por ejemplo, `db.Patients.Count + 1`) y hacer `db.Patients.Add(this)`.

Buenas prácticas al extender
---------------------------
- Mantén la lógica de I/O (consola) separada de la lógica de negocio. Los menús solo deben llamar a managers.
- Para pruebas unitarias, mueve la lógica de negocio (validaciones, búsquedas, asignación de Ids) a los managers o a un repositorio y haz que los modelos sean POCOs ligeros.
- Añade pruebas unitarias (xUnit / NUnit) que instancien `DatabaseContent` en memoria y verifiquen los métodos de `PatientManager`/`DoctorManager`.

Notas y detalles útiles
----------------------
- Se targetea `net8.0`.
- Si añades un constructor parametrizado a `Person`, preserva un constructor sin parámetros si hay código que lo necesita.
- Para evitar bloqueos en CI o cuando usas tuberías, los menús usan `Console.IsInputRedirected` para condicionar llamadas a `Console.ReadKey()`.

Siguientes pasos sugeridos
-------------------------
- Añadir un `README.md` en raíz con resumen y comandos básicos (puedo generarlo ahora si quieres).
- Añadir pruebas unitarias para `PatientManager`.
- Reemplazar `DatabaseContent` por una capa de repositorio persistente (opcional).

Contacto rápido
---------------
Si quieres que expanda el documento (incluir ejemplos de código, diagramas simples o instrucciones para tests), dime qué se debe enfatizar y lo agrego.