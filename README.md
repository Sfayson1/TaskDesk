# Project Name: TaskDesk — Client & Invoice Tracker

## Project Description

TaskDesk is a terminal-based client and invoice tracking application written in C#. It
allows a freelancer or small business to manage clients, projects, tasks, and billable
time entries, then generate formatted invoice summaries — all persisted to a local SQLite
database. The project was designed to demonstrate core Object-Oriented Programming concepts
including interfaces, abstract classes, inheritance, composition, and polymorphism.

## Project Tasks

- **Task 1: Project Proposal (Week 1)**
  - Chose a realistic business domain (freelance client/invoice tracking)
  - Defined the application's purpose and scope
  - Submitted Word document proposal

- **Task 2: Application Design (Week 2)**
  - Identified all required classes and their relationships
  - Designed the four-table SQLite schema (Clients, Projects, Tasks, TimeEntries)
  - Mapped OOP requirements to specific classes
  - Submitted design document

- **Task 3: Class Implementation (Week 3)**
  - Created `BaseEntity` abstract class with shared `Id`, `CreatedAt`, and abstract `GetInfo()`
  - Created `IBillable` interface with `CalculateTotal()`
  - Implemented `Client`, `Project`, `TaskItem`, `TimeEntry` classes inheriting from `BaseEntity`
  - Implemented `Invoice` class demonstrating composition
  - Added constructors and access specifiers to all classes

- **Task 4: Database Implementation (Week 4)**
  - Added `Microsoft.Data.Sqlite` NuGet package
  - Built `DatabaseManager` static class with full CRUD for all four entities
  - Implemented parameterized queries to prevent SQL injection
  - Wired database operations into menu-driven terminal interface in `Program.cs`

- **Task 5: Final Submission (Week 5)**
  - Wrote project summary and requirements mapping document
  - Pushed complete source code to GitHub
  - Submitted GitHub URL with Word documents

## Project Skills Learned

- Object-Oriented Programming in C# (interfaces, abstract classes, inheritance, polymorphism, composition)
- SQLite database design and CRUD operations with `Microsoft.Data.Sqlite`
- Parameterized SQL queries and SQL injection prevention
- Terminal UI design with menu-driven navigation using `Console.ReadLine` / `Console.WriteLine`
- Input validation and error handling in C#
- Version control with Git and GitHub
- Software design documentation (proposal, class diagrams, data model)

## Language Used

- **C#**: Application logic, OOP class hierarchy, terminal interface
- **SQLite**: Persistent local data storage via `Microsoft.Data.Sqlite`
- **SQL**: Table schema creation and CRUD queries

## Development Process Used

- **Phased / Iterative Development**: Each week's submission built on the previous phase —
  proposal → design → class framework → database integration → final documentation.

## Notes

- Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download) or later.
- Restore dependencies before running:

  ```bash
  dotnet restore
  ```

- Run the application:

  ```bash
  dotnet run
  ```
  
- The SQLite database file (`taskdesk.db`) is created automatically in the project directory
  on first run. No manual database setup is needed.
- **Dependencies:**
  - `Microsoft.Data.Sqlite` (NuGet)
  - `SQLitePCLRaw.bundle_e_sqlite3` (pulled in transitively)

## OOP Requirements Mapping

| Requirement | Design Element | Implementation File |
| --- | --- | --- |
| Interface class | `IBillable` | `IBillable.cs` |
| Abstract class | `BaseEntity` | `BaseEntity.cs` |
| Composition | `Invoice` owns `Client`, `Project`, `List<TimeEntry>` | `Invoice.cs` |
| Polymorphism | `GetInfo()` overridden in all four entity classes; `IBillable.CalculateTotal()` implemented by `TimeEntry` | `Client.cs`, `Project.cs`, `TaskItem.cs`, `TimeEntry.cs` |
| Constructors | Parameterized constructors on every class | All `.cs` files |
| Access specifiers | `private` fields, `public` properties, `protected` base constructor | `BaseEntity.cs` and all subclasses |
| Terminal I/O | Menu-driven interface with `Console.ReadLine` / `Console.WriteLine` | `Program.cs` |
| SQLite CRUD | `CreateClient`, `GetClients`, `UpdateClient`, `DeleteClient` (and equivalents for all entities) | `DatabaseManager.cs` |

## Demo Video

[Watch the Demo on YouTube](https://youtu.be/n0I-SYUVnZc)

## Link to Project

[TaskDesk Repository](https://github.com/Sfayson1/TaskDesk)

## License

This project is licensed under the MIT License.

---

## Project Summary

### What the Application Does

TaskDesk is a menu-driven terminal application that helps a freelancer or small business
track clients, projects, tasks, and billable time — and generate formatted invoice summaries
on demand. All data is saved to a local SQLite database so records persist between sessions.
A typical workflow is: add a client, create a project for that client, log time entries
against the project, and then generate an invoice that totals the billable hours.

### What Went Well

- **OOP design came together cleanly.** The `BaseEntity` abstract class gave every entity
  a consistent `Id`, `CreatedAt`, and polymorphic `GetInfo()` with almost no repeated code.
  Once that base was solid, adding `Client`, `Project`, `TaskItem`, and `TimeEntry` was
  straightforward.
- **The `Invoice` composition class.** Wrapping a `Client`, a `Project`, and a
  `List<TimeEntry>` inside a single `Invoice` object made the invoice generation logic
  intuitive and kept `Program.cs` readable.
- **SQLite integration.** Using `Microsoft.Data.Sqlite` with parameterized queries was
  simpler than expected. The `DatabaseManager` static class kept all SQL in one place and
  the schema initialized reliably on first run via `CREATE TABLE IF NOT EXISTS`.
- **Input validation.** Adding `int.TryParse` and `decimal.TryParse` guards throughout
  `Program.cs` prevented crashes from bad user input without a lot of extra code.

### What Was Challenging

- **Connecting the class layer to the database layer.** Early on, the entity constructors
  didn't set `Id` or `CreatedAt` from the database — those had to be assigned after the
  fact using `SELECT last_insert_rowid()` and manual property assignment when reading rows.
  It took a few iterations to make that pattern consistent across all four entity types.
- **`TimeEntry` update in the UI.** `DatabaseManager.UpdateTimeEntry()` was implemented in
  the database layer first, then wired to an "Edit Time Entry" menu option in the Manage
  Time Entries submenu. The menu prompts the user to update hours worked, hourly rate, and
  work description individually, keeping the existing value for any field left blank.
- **Deciding where polymorphism was truly at work.** The `GetInfo()` override pattern is
  clear polymorphism, but it required thinking carefully about *why* it matters: any
  `BaseEntity` reference could call `GetInfo()` and get the right output for whichever
  subtype was stored there. The `IBillable` interface reinforced this by letting
  `Invoice.CalculateInvoiceTotal()` call `entry.CalculateTotal()` through the interface
  contract rather than knowing the concrete type.

### How the Project Satisfies Each Requirement

| Requirement | How It Is Satisfied |
| --- | --- |
| Terminal input/output | `Program.cs` — full menu-driven interface reads user input with `Console.ReadLine` and writes all output with `Console.WriteLine` |
| In-code comments | Every `.cs` file has a header block comment (author, date, assignment, description) and section dividers |
| Interface class | `IBillable` (IBillable.cs) declares `CalculateTotal()`; implemented by `TimeEntry` |
| Abstract class | `BaseEntity` (BaseEntity.cs) is `abstract`, declares abstract `GetInfo()`, provides shared `Id` and `CreatedAt` |
| Composition | `Invoice` (Invoice.cs) contains a `Client`, a `Project`, and a `List<TimeEntry>` as member fields |
| Polymorphism | `GetInfo()` is overridden in `Client`, `Project`, `TaskItem`, and `TimeEntry`; `IBillable.CalculateTotal()` is called through the interface in `Invoice.CalculateInvoiceTotal()` |
| Constructors | Every class has at least one parameterized constructor; `BaseEntity` has a `protected` constructor |
| Access specifiers | All fields are `private`; properties are `public`; `BaseEntity` constructor is `protected`; `DatabaseManager` methods are `public static` |
| SQLite CRUD | `DatabaseManager.cs` provides Create, Read (all + by ID + filtered), Update, and Delete for `Client`, `Project`, `TaskItem`, and `TimeEntry` — 16+ database methods total |

### Design-to-Implementation Mapping

| Design Element | Class / File | Role |
| --- | --- | --- |
| Abstract base entity | `BaseEntity` / BaseEntity.cs | Parent of all model classes |
| Billing interface | `IBillable` / IBillable.cs | Enforces `CalculateTotal()` contract |
| Client record | `Client` / Client.cs | Stores contact info; inherits BaseEntity |
| Project record | `Project` / Project.cs | Linked to a client via `ClientId`; stores rate and status |
| Task record | `TaskItem` / TaskItem.cs | Linked to a project; tracks completion state |
| Time entry record | `TimeEntry` / TimeEntry.cs | Billable hours; implements IBillable |
| Invoice generator | `Invoice` / Invoice.cs | Composes Client + Project + TimeEntries; formats invoice output |
| Data access layer | `DatabaseManager` / DatabaseManager.cs | All SQL; full CRUD for all four tables |
| Application entry point | `Program` / Program.cs | Menu-driven UI; wires user input to DatabaseManager calls |
