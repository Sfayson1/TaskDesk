/*
Name: Sherika Fayson
Date: May 31, 2026
Assignment: SDC320 Course Project - Phase 1 Class Implementation
Description: Static utility class that handles all communication between the application
             and the SQLite database. Contains static CRUD methods for every entity and
             initializes the database schema on first run. Full SQLite implementation
             will be completed in Phase 2 (Week 4).
*/

using System.Collections.Generic;

public static class DatabaseManager
{
  public static void InitializeDatabase()
  {
    // SQLite implementation coming in Phase 2
  }

  // ==================== CLIENT METHODS ====================

  public static void CreateClient(Client client)
  {
    // SQLite implementation coming in Phase 2
  }

  public static List<Client> GetClients()
  {
    // SQLite implementation coming in Phase 2
    return new List<Client>();
  }

  public static Client GetClientById(int id)
  {
    // SQLite implementation coming in Phase 2
    return null;
  }

  public static void UpdateClient(Client client)
  {
    // SQLite implementation coming in Phase 2
  }

  public static void DeleteClient(int clientId)
  {
    // SQLite implementation coming in Phase 2
  }

  // ==================== PROJECT METHODS ====================

  public static void CreateProject(Project project)
  {
    // SQLite implementation coming in Phase 2
  }

  public static List<Project> GetProjects()
  {
    // SQLite implementation coming in Phase 2
    return new List<Project>();
  }

  public static List<Project> GetProjectsByClient(int clientId)
  {
    // SQLite implementation coming in Phase 2
    return new List<Project>();
  }

  public static void UpdateProject(Project project)
  {
    // SQLite implementation coming in Phase 2
  }

  public static void DeleteProject(int projectId)
  {
    // SQLite implementation coming in Phase 2
  }

  // ==================== TASK METHODS ====================

  public static void CreateTask(TaskItem task)
  {
    // SQLite implementation coming in Phase 2
  }

  public static List<TaskItem> GetTasksByProject(int projectId)
  {
    // SQLite implementation coming in Phase 2
    return new List<TaskItem>();
  }

  public static void UpdateTask(TaskItem task)
  {
    // SQLite implementation coming in Phase 2
  }

  public static void DeleteTask(int taskId)
  {
    // SQLite implementation coming in Phase 2
  }

  // ==================== TIME ENTRY METHODS ====================

  public static void CreateTimeEntry(TimeEntry entry)
  {
    // SQLite implementation coming in Phase 2
  }

  public static List<TimeEntry> GetTimeEntriesByProject(int projectId)
  {
    // SQLite implementation coming in Phase 2
    return new List<TimeEntry>();
  }

  public static void UpdateTimeEntry(TimeEntry entry)
  {
    // SQLite implementation coming in Phase 2
  }

  public static void DeleteTimeEntry(int entryId)
  {
    // SQLite implementation coming in Phase 2
  }
}
