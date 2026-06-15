/*
Name: Sherika Fayson
Date: June 7, 2026
Assignment: SDC320 Course Project - Phase 2 Database Implementation
Description: Main entry point for the TaskDesk Client & Invoice Tracker application.
             Provides a menu-driven terminal interface for managing clients, projects,
             tasks, time entries, and generating formatted invoice summaries.
             All data is persisted to a SQLite database via DatabaseManager.
*/

using System;
using System.Collections.Generic;

class Program
{
  static void Main()
  {
    DatabaseManager.InitializeDatabase();

    Console.WriteLine("=========================================");
    Console.WriteLine("   TaskDesk Client & Invoice Tracker");
    Console.WriteLine("   Sherika Fayson");
    Console.WriteLine("=========================================");
    Console.WriteLine();

    bool running = true;
    while (running)
    {
      ShowMainMenu();
      string choice = Console.ReadLine();

      switch (choice)
      {
        case "1":
          ManageClients();
          break;
        case "2":
          ManageProjects();
          break;
        case "3":
          ManageTasks();
          break;
        case "4":
          ManageTimeEntries();
          break;
        case "5":
          GenerateInvoice();
          break;
        case "6":
          running = false;
          Console.WriteLine("\nThank you for using TaskDesk. Goodbye!");
          break;
        default:
          Console.WriteLine("\nInvalid option. Please try again.");
          break;
      }
    }
  }

  static void ShowMainMenu()
  {
    Console.WriteLine("\n----- MAIN MENU -----");
    Console.WriteLine("1. Manage Clients");
    Console.WriteLine("2. Manage Projects");
    Console.WriteLine("3. Manage Tasks");
    Console.WriteLine("4. Manage Time Entries");
    Console.WriteLine("5. Generate Invoice");
    Console.WriteLine("6. Exit");
    Console.Write("Select an option: ");
  }

  // ==================== CLIENT MANAGEMENT ====================

  static void ManageClients()
  {
    bool back = false;
    while (!back)
    {
      Console.WriteLine("\n----- MANAGE CLIENTS -----");
      Console.WriteLine("1. Add Client");
      Console.WriteLine("2. View All Clients");
      Console.WriteLine("3. Update Client");
      Console.WriteLine("4. Delete Client");
      Console.WriteLine("5. Back to Main Menu");
      Console.Write("Select an option: ");

      string choice = Console.ReadLine();
      switch (choice)
      {
        case "1":
          AddClient();
          break;
        case "2":
          ViewClients();
          break;
        case "3":
          UpdateClient();
          break;
        case "4":
          DeleteClient();
          break;
        case "5":
          back = true;
          break;
        default:
          Console.WriteLine("Invalid option.");
          break;
      }
    }
  }

  static void AddClient()
  {
    Console.WriteLine("\n--- Add New Client ---");
    Console.Write("Name: ");
    string name = Console.ReadLine();

    Console.Write("Email: ");
    string email = Console.ReadLine();

    Console.Write("Phone Number: ");
    string phone = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
    {
      Console.WriteLine("Error: Name and email are required.");
      return;
    }

    Client client = new Client(name, email, phone);
    DatabaseManager.CreateClient(client);
    Console.WriteLine($"\nClient added successfully!");
    Console.WriteLine(client.GetInfo());
  }

  static void ViewClients()
  {
    Console.WriteLine("\n--- All Clients ---");
    List<Client> clients = DatabaseManager.GetClients();

    if (clients.Count == 0)
    {
      Console.WriteLine("No clients found.");
      return;
    }

    foreach (Client client in clients)
    {
      Console.WriteLine();
      Console.WriteLine(client.ToString());
      Console.WriteLine("------------------");
    }
  }

  static void UpdateClient()
  {
    Console.WriteLine("\n--- Update Client ---");
    List<Client> clients = DatabaseManager.GetClients();

    if (clients.Count == 0)
    {
      Console.WriteLine("No clients found.");
      return;
    }

    foreach (Client c in clients)
    {
      Console.WriteLine();
      Console.WriteLine(c.ToString());
      Console.WriteLine("------------------");
    }

    Console.Write("Enter Client ID to update: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
      Console.WriteLine("Invalid ID.");
      return;
    }

    Client client = DatabaseManager.GetClientById(id);
    if (client == null)
    {
      Console.WriteLine("Client not found.");
      return;
    }

    Console.Write($"New Name ({client.Name}): ");
    string name = Console.ReadLine();

    Console.Write($"New Email ({client.Email}): ");
    string email = Console.ReadLine();

    Console.Write($"New Phone ({client.PhoneNumber}): ");
    string phone = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(name)) client.Name = name;
    if (!string.IsNullOrWhiteSpace(email)) client.Email = email;
    if (!string.IsNullOrWhiteSpace(phone)) client.PhoneNumber = phone;

    DatabaseManager.UpdateClient(client);
    Console.WriteLine($"\nClient updated successfully!");
    Console.WriteLine(client.GetInfo());
  }

  static void DeleteClient()
  {
    Console.WriteLine("\n--- Delete Client ---");
    List<Client> clients = DatabaseManager.GetClients();

    if (clients.Count == 0)
    {
      Console.WriteLine("No clients found.");
      return;
    }

    foreach (Client c in clients)
    {
      Console.WriteLine();
      Console.WriteLine(c.ToString());
      Console.WriteLine("------------------");
    }

    Console.Write("Enter Client ID to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
      Console.WriteLine("Invalid ID.");
      return;
    }

    Client client = DatabaseManager.GetClientById(id);
    if (client == null)
    {
      Console.WriteLine("Client not found.");
      return;
    }

    DatabaseManager.DeleteClient(id);
    Console.WriteLine($"Client '{client.Name}' deleted successfully.");
  }

  // ==================== PROJECT MANAGEMENT ====================

  static void ManageProjects()
  {
    bool back = false;
    while (!back)
    {
      Console.WriteLine("\n----- MANAGE PROJECTS -----");
      Console.WriteLine("1. Add Project");
      Console.WriteLine("2. View All Projects");
      Console.WriteLine("3. Update Project");
      Console.WriteLine("4. Delete Project");
      Console.WriteLine("5. Back to Main Menu");
      Console.Write("Select an option: ");

      string choice = Console.ReadLine();
      switch (choice)
      {
        case "1":
          AddProject();
          break;
        case "2":
          ViewProjects();
          break;
        case "3":
          UpdateProject();
          break;
        case "4":
          DeleteProject();
          break;
        case "5":
          back = true;
          break;
        default:
          Console.WriteLine("Invalid option.");
          break;
      }
    }
  }

  static void AddProject()
  {
    Console.WriteLine("\n--- Add New Project ---");
    List<Client> clients = DatabaseManager.GetClients();

    if (clients.Count == 0)
    {
      Console.WriteLine("No clients available. Please add a client first.");
      return;
    }

    foreach (Client c in clients)
    {
      Console.WriteLine(c.GetInfo());
    }

    Console.Write("Enter Client ID for this project: ");
    if (!int.TryParse(Console.ReadLine(), out int clientId) || DatabaseManager.GetClientById(clientId) == null)
    {
      Console.WriteLine("Invalid Client ID.");
      return;
    }

    Console.Write("Project Name: ");
    string name = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(name))
    {
      Console.WriteLine("Error: Project name is required.");
      return;
    }

    Console.Write("Hourly Rate ($): ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal rate) || rate < 0)
    {
      Console.WriteLine("Invalid rate.");
      return;
    }

    Console.Write("Status (Active/On Hold/Completed): ");
    string status = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(status)) status = "Active";

    Project project = new Project(name, rate, status, clientId);
    DatabaseManager.CreateProject(project);
    Console.WriteLine($"\nProject added successfully!");
    Console.WriteLine(project.GetInfo());
  }

  static void ViewProjects()
  {
    Console.WriteLine("\n--- All Projects ---");
    List<Project> projects = DatabaseManager.GetProjects();

    if (projects.Count == 0)
    {
      Console.WriteLine("No projects found.");
      return;
    }

    foreach (Project project in projects)
    {
      Console.WriteLine();
      Console.WriteLine(project.ToString());
      Console.WriteLine("------------------");
    }
  }

  static void UpdateProject()
  {
    Console.WriteLine("\n--- Update Project ---");
    List<Project> projects = DatabaseManager.GetProjects();

    if (projects.Count == 0)
    {
      Console.WriteLine("No projects found.");
      return;
    }

    foreach (Project p in projects)
    {
      Console.WriteLine();
      Console.WriteLine(p.ToString());
      Console.WriteLine("------------------");
    }

    Console.Write("Enter Project ID to update: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
      Console.WriteLine("Invalid ID.");
      return;
    }

    Project project = DatabaseManager.GetProjectById(id);
    if (project == null)
    {
      Console.WriteLine("Project not found.");
      return;
    }

    Console.Write($"New Name ({project.ProjectName}): ");
    string name = Console.ReadLine();

    Console.Write($"New Status ({project.Status}): ");
    string status = Console.ReadLine();

    Console.Write($"New Hourly Rate (${project.HourlyRate:F2}): ");
    string rateInput = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(name)) project.ProjectName = name;
    if (!string.IsNullOrWhiteSpace(status)) project.Status = status;
    if (!string.IsNullOrWhiteSpace(rateInput) && decimal.TryParse(rateInput, out decimal rate))
    {
      project.HourlyRate = rate;
    }

    DatabaseManager.UpdateProject(project);
    Console.WriteLine($"\nProject updated successfully!");
    Console.WriteLine(project.GetInfo());
  }

  static void DeleteProject()
  {
    Console.WriteLine("\n--- Delete Project ---");
    List<Project> projects = DatabaseManager.GetProjects();

    if (projects.Count == 0)
    {
      Console.WriteLine("No projects found.");
      return;
    }

    foreach (Project p in projects)
    {
      Console.WriteLine();
      Console.WriteLine(p.ToString());
      Console.WriteLine("------------------");
    }

    Console.Write("Enter Project ID to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
      Console.WriteLine("Invalid ID.");
      return;
    }

    Project project = DatabaseManager.GetProjectById(id);
    if (project == null)
    {
      Console.WriteLine("Project not found.");
      return;
    }

    DatabaseManager.DeleteProject(id);
    Console.WriteLine($"Project '{project.ProjectName}' deleted successfully.");
  }

  // ==================== TASK MANAGEMENT ====================

  static void ManageTasks()
  {
    bool back = false;
    while (!back)
    {
      Console.WriteLine("\n----- MANAGE TASKS -----");
      Console.WriteLine("1. Add Task");
      Console.WriteLine("2. View All Tasks");
      Console.WriteLine("3. Mark Task Complete");
      Console.WriteLine("4. Delete Task");
      Console.WriteLine("5. Back to Main Menu");
      Console.Write("Select an option: ");

      string choice = Console.ReadLine();
      switch (choice)
      {
        case "1":
          AddTask();
          break;
        case "2":
          ViewTasks();
          break;
        case "3":
          MarkTaskComplete();
          break;
        case "4":
          DeleteTask();
          break;
        case "5":
          back = true;
          break;
        default:
          Console.WriteLine("Invalid option.");
          break;
      }
    }
  }

  static void AddTask()
  {
    Console.WriteLine("\n--- Add New Task ---");
    List<Project> projects = DatabaseManager.GetProjects();

    if (projects.Count == 0)
    {
      Console.WriteLine("No projects available. Please add a project first.");
      return;
    }

    foreach (Project p in projects)
    {
      Console.WriteLine(p.GetInfo());
    }

    Console.Write("Enter Project ID for this task: ");
    if (!int.TryParse(Console.ReadLine(), out int projectId) || DatabaseManager.GetProjectById(projectId) == null)
    {
      Console.WriteLine("Invalid Project ID.");
      return;
    }

    Console.Write("Task Title: ");
    string title = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(title))
    {
      Console.WriteLine("Error: Task title is required.");
      return;
    }

    Console.Write("Description: ");
    string description = Console.ReadLine();

    TaskItem task = new TaskItem(title, description, projectId);
    DatabaseManager.CreateTask(task);
    Console.WriteLine($"\nTask added successfully!");
    Console.WriteLine(task.GetInfo());
  }

  static void ViewTasks()
  {
    Console.WriteLine("\n--- All Tasks ---");
    List<TaskItem> tasks = DatabaseManager.GetAllTasks();

    if (tasks.Count == 0)
    {
      Console.WriteLine("No tasks found.");
      return;
    }

    foreach (TaskItem task in tasks)
    {
      Console.WriteLine();
      Console.WriteLine(task.ToString());
      Console.WriteLine("------------------");
    }
  }

  static void MarkTaskComplete()
  {
    Console.WriteLine("\n--- Mark Task Complete ---");
    List<TaskItem> tasks = DatabaseManager.GetAllTasks();

    if (tasks.Count == 0)
    {
      Console.WriteLine("No tasks found.");
      return;
    }

    foreach (TaskItem t in tasks)
    {
      Console.WriteLine();
      Console.WriteLine(t.ToString());
      Console.WriteLine("------------------");
    }

    Console.Write("Enter Task ID to mark complete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
      Console.WriteLine("Invalid ID.");
      return;
    }

    TaskItem task = DatabaseManager.GetTaskById(id);
    if (task == null)
    {
      Console.WriteLine("Task not found.");
      return;
    }

    task.MarkComplete();
    DatabaseManager.UpdateTask(task);
    Console.WriteLine($"\nTask marked as complete!");
    Console.WriteLine(task.GetInfo());
  }

  static void DeleteTask()
  {
    Console.WriteLine("\n--- Delete Task ---");
    List<TaskItem> tasks = DatabaseManager.GetAllTasks();

    if (tasks.Count == 0)
    {
      Console.WriteLine("No tasks found.");
      return;
    }

    foreach (TaskItem t in tasks)
    {
      Console.WriteLine();
      Console.WriteLine(t.ToString());
      Console.WriteLine("------------------");
    }

    Console.Write("Enter Task ID to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
      Console.WriteLine("Invalid ID.");
      return;
    }

    TaskItem task = DatabaseManager.GetTaskById(id);
    if (task == null)
    {
      Console.WriteLine("Task not found.");
      return;
    }

    DatabaseManager.DeleteTask(id);
    Console.WriteLine($"Task '{task.Title}' deleted successfully.");
  }

  // ==================== TIME ENTRY MANAGEMENT ====================

  static void ManageTimeEntries()
  {
    bool back = false;
    while (!back)
    {
      Console.WriteLine("\n----- MANAGE TIME ENTRIES -----");
      Console.WriteLine("1. Add Time Entry");
      Console.WriteLine("2. View All Time Entries");
      Console.WriteLine("3. Edit Time Entry");
      Console.WriteLine("4. Delete Time Entry");
      Console.WriteLine("5. Back to Main Menu");
      Console.Write("Select an option: ");

      string choice = Console.ReadLine();
      switch (choice)
      {
        case "1":
          AddTimeEntry();
          break;
        case "2":
          ViewTimeEntries();
          break;
        case "3":
          EditTimeEntry();
          break;
        case "4":
          DeleteTimeEntry();
          break;
        case "5":
          back = true;
          break;
        default:
          Console.WriteLine("Invalid option.");
          break;
      }
    }
  }

  static void AddTimeEntry()
  {
    Console.WriteLine("\n--- Add Time Entry ---");
    List<Project> projects = DatabaseManager.GetProjects();

    if (projects.Count == 0)
    {
      Console.WriteLine("No projects available. Please add a project first.");
      return;
    }

    foreach (Project p in projects)
    {
      Console.WriteLine(p.GetInfo());
    }

    Console.Write("Enter Project ID: ");
    if (!int.TryParse(Console.ReadLine(), out int projectId))
    {
      Console.WriteLine("Invalid Project ID.");
      return;
    }

    Project project = DatabaseManager.GetProjectById(projectId);
    if (project == null)
    {
      Console.WriteLine("Project not found.");
      return;
    }

    Console.Write("Hours Worked: ");
    if (!double.TryParse(Console.ReadLine(), out double hours) || hours <= 0)
    {
      Console.WriteLine("Invalid hours.");
      return;
    }

    Console.Write($"Hourly Rate (press Enter for project default ${project.HourlyRate:F2}): ");
    string rateInput = Console.ReadLine();
    decimal rate = string.IsNullOrWhiteSpace(rateInput) ? project.HourlyRate : decimal.Parse(rateInput);

    Console.Write("Work Description: ");
    string description = Console.ReadLine();

    TimeEntry entry = new TimeEntry(hours, rate, description, projectId);
    DatabaseManager.CreateTimeEntry(entry);
    Console.WriteLine($"\nTime entry added successfully!");
    Console.WriteLine(entry.GetInfo());
  }

  static void ViewTimeEntries()
  {
    Console.WriteLine("\n--- All Time Entries ---");
    List<TimeEntry> entries = DatabaseManager.GetAllTimeEntries();

    if (entries.Count == 0)
    {
      Console.WriteLine("No time entries found.");
      return;
    }

    foreach (TimeEntry entry in entries)
    {
      Console.WriteLine();
      Console.WriteLine(entry.ToString());
      Console.WriteLine("------------------");
    }
  }

  static void EditTimeEntry()
  {
    Console.WriteLine("\n--- Edit Time Entry ---");
    List<TimeEntry> entries = DatabaseManager.GetAllTimeEntries();

    if (entries.Count == 0)
    {
      Console.WriteLine("No time entries found.");
      return;
    }

    foreach (TimeEntry e in entries)
    {
      Console.WriteLine();
      Console.WriteLine(e.ToString());
      Console.WriteLine("------------------");
    }

    Console.Write("Enter Time Entry ID to edit: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
      Console.WriteLine("Invalid ID.");
      return;
    }

    TimeEntry entry = DatabaseManager.GetTimeEntryById(id);
    if (entry == null)
    {
      Console.WriteLine("Time entry not found.");
      return;
    }

    Console.Write($"New Hours Worked ({entry.HoursWorked}): ");
    string hoursInput = Console.ReadLine();

    Console.Write($"New Hourly Rate (${entry.HourlyRate:F2}): ");
    string rateInput = Console.ReadLine();

    Console.Write($"New Description ({entry.WorkDescription}): ");
    string description = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(hoursInput) && double.TryParse(hoursInput, out double hours) && hours > 0)
      entry.HoursWorked = hours;
    if (!string.IsNullOrWhiteSpace(rateInput) && decimal.TryParse(rateInput, out decimal rate) && rate >= 0)
      entry.HourlyRate = rate;
    if (!string.IsNullOrWhiteSpace(description))
      entry.WorkDescription = description;

    DatabaseManager.UpdateTimeEntry(entry);
    Console.WriteLine("\nTime entry updated successfully!");
    Console.WriteLine(entry.GetInfo());
  }

  static void DeleteTimeEntry()
  {
    Console.WriteLine("\n--- Delete Time Entry ---");
    List<TimeEntry> entries = DatabaseManager.GetAllTimeEntries();

    if (entries.Count == 0)
    {
      Console.WriteLine("No time entries found.");
      return;
    }

    foreach (TimeEntry e in entries)
    {
      Console.WriteLine();
      Console.WriteLine(e.ToString());
      Console.WriteLine("------------------");
    }

    Console.Write("Enter Time Entry ID to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
      Console.WriteLine("Invalid ID.");
      return;
    }

    TimeEntry entry = DatabaseManager.GetTimeEntryById(id);
    if (entry == null)
    {
      Console.WriteLine("Time entry not found.");
      return;
    }

    DatabaseManager.DeleteTimeEntry(id);
    Console.WriteLine("Time entry deleted successfully.");
  }

  // ==================== INVOICE GENERATION ====================

  static void GenerateInvoice()
  {
    Console.WriteLine("\n----- GENERATE INVOICE -----");

    List<Client> clients = DatabaseManager.GetClients();
    if (clients.Count == 0)
    {
      Console.WriteLine("No clients available. Please add a client first.");
      return;
    }

    List<Project> allProjects = DatabaseManager.GetProjects();
    if (allProjects.Count == 0)
    {
      Console.WriteLine("No projects available. Please add a project first.");
      return;
    }

    foreach (Client c in clients)
    {
      Console.WriteLine(c.GetInfo());
    }

    Console.Write("Enter Client ID: ");
    if (!int.TryParse(Console.ReadLine(), out int clientId))
    {
      Console.WriteLine("Invalid ID.");
      return;
    }

    Client client = DatabaseManager.GetClientById(clientId);
    if (client == null)
    {
      Console.WriteLine("Client not found.");
      return;
    }

    List<Project> clientProjects = DatabaseManager.GetProjectsByClient(clientId);
    if (clientProjects.Count == 0)
    {
      Console.WriteLine("No projects found for this client.");
      return;
    }

    Console.WriteLine("\nProjects for this client:");
    foreach (Project p in clientProjects)
    {
      Console.WriteLine(p.GetInfo());
    }

    Console.Write("Enter Project ID: ");
    if (!int.TryParse(Console.ReadLine(), out int projectId))
    {
      Console.WriteLine("Invalid ID.");
      return;
    }

    Project project = DatabaseManager.GetProjectById(projectId);
    if (project == null || project.ClientId != clientId)
    {
      Console.WriteLine("Project not found.");
      return;
    }

    List<TimeEntry> projectEntries = DatabaseManager.GetTimeEntriesByProject(projectId);
    Invoice invoice = new Invoice(client, project, projectEntries);

    Console.WriteLine();
    Console.WriteLine(invoice.GenerateInvoiceSummary());
  }
}
