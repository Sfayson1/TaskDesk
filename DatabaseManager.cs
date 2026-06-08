/*
Name: Sherika Fayson
Date: June 7, 2026
Assignment: SDC320 Course Project - Phase 2 Database Implementation
Description: Static utility class that handles all communication between the application
             and the SQLite database. Initializes the schema on first run and provides
             full CRUD operations for Client, Project, TaskItem, and TimeEntry entities.
             Uses Microsoft.Data.Sqlite with parameterized queries to prevent SQL injection.
*/

using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

public static class DatabaseManager
{
  private static readonly string ConnectionString = "Data Source=taskdesk.db";

  public static void InitializeDatabase()
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      string sql = @"
        PRAGMA foreign_keys = ON;

        CREATE TABLE IF NOT EXISTS Clients (
          Id          INTEGER PRIMARY KEY AUTOINCREMENT,
          Name        TEXT    NOT NULL,
          Email       TEXT    NOT NULL,
          PhoneNumber TEXT    NOT NULL,
          CreatedAt   TEXT    NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Projects (
          Id          INTEGER PRIMARY KEY AUTOINCREMENT,
          ProjectName TEXT    NOT NULL,
          HourlyRate  REAL    NOT NULL,
          Status      TEXT    NOT NULL,
          ClientId    INTEGER NOT NULL,
          CreatedAt   TEXT    NOT NULL,
          FOREIGN KEY (ClientId) REFERENCES Clients(Id)
        );

        CREATE TABLE IF NOT EXISTS Tasks (
          Id          INTEGER PRIMARY KEY AUTOINCREMENT,
          Title       TEXT    NOT NULL,
          Description TEXT    NOT NULL,
          IsCompleted INTEGER NOT NULL DEFAULT 0,
          ProjectId   INTEGER NOT NULL,
          CreatedAt   TEXT    NOT NULL,
          FOREIGN KEY (ProjectId) REFERENCES Projects(Id)
        );

        CREATE TABLE IF NOT EXISTS TimeEntries (
          Id              INTEGER PRIMARY KEY AUTOINCREMENT,
          HoursWorked     REAL    NOT NULL,
          HourlyRate      REAL    NOT NULL,
          WorkDescription TEXT    NOT NULL,
          ProjectId       INTEGER NOT NULL,
          CreatedAt       TEXT    NOT NULL,
          FOREIGN KEY (ProjectId) REFERENCES Projects(Id)
        );
      ";

      using (SqliteCommand cmd = new SqliteCommand(sql, conn))
      {
        cmd.ExecuteNonQuery();
      }
    }
  }

  // ==================== CLIENT METHODS ====================

  public static void CreateClient(Client client)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "INSERT INTO Clients (Name, Email, PhoneNumber, CreatedAt) VALUES (@name, @email, @phone, @created)",
        conn))
      {
        cmd.Parameters.AddWithValue("@name", client.Name);
        cmd.Parameters.AddWithValue("@email", client.Email);
        cmd.Parameters.AddWithValue("@phone", client.PhoneNumber ?? "");
        cmd.Parameters.AddWithValue("@created", client.CreatedAt.ToString("o"));
        cmd.ExecuteNonQuery();
      }

      using (SqliteCommand lastId = new SqliteCommand("SELECT last_insert_rowid()", conn))
      {
        client.Id = (int)(long)lastId.ExecuteScalar();
      }
    }
  }

  public static List<Client> GetClients()
  {
    List<Client> clients = new List<Client>();

    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "SELECT Id, Name, Email, PhoneNumber, CreatedAt FROM Clients ORDER BY Id",
        conn))
      using (SqliteDataReader reader = cmd.ExecuteReader())
      {
        while (reader.Read())
        {
          Client client = new Client(
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3)
          );
          client.Id = reader.GetInt32(0);
          client.CreatedAt = DateTime.Parse(reader.GetString(4));
          clients.Add(client);
        }
      }
    }

    return clients;
  }

  public static Client GetClientById(int id)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "SELECT Id, Name, Email, PhoneNumber, CreatedAt FROM Clients WHERE Id = @id",
        conn))
      {
        cmd.Parameters.AddWithValue("@id", id);

        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
          if (reader.Read())
          {
            Client client = new Client(
              reader.GetString(1),
              reader.GetString(2),
              reader.GetString(3)
            );
            client.Id = reader.GetInt32(0);
            client.CreatedAt = DateTime.Parse(reader.GetString(4));
            return client;
          }
        }
      }
    }

    return null;
  }

  public static void UpdateClient(Client client)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "UPDATE Clients SET Name = @name, Email = @email, PhoneNumber = @phone WHERE Id = @id",
        conn))
      {
        cmd.Parameters.AddWithValue("@name", client.Name);
        cmd.Parameters.AddWithValue("@email", client.Email);
        cmd.Parameters.AddWithValue("@phone", client.PhoneNumber ?? "");
        cmd.Parameters.AddWithValue("@id", client.Id);
        cmd.ExecuteNonQuery();
      }
    }
  }

  public static void DeleteClient(int clientId)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand("DELETE FROM Clients WHERE Id = @id", conn))
      {
        cmd.Parameters.AddWithValue("@id", clientId);
        cmd.ExecuteNonQuery();
      }
    }
  }

  // ==================== PROJECT METHODS ====================

  public static void CreateProject(Project project)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "INSERT INTO Projects (ProjectName, HourlyRate, Status, ClientId, CreatedAt) VALUES (@name, @rate, @status, @clientId, @created)",
        conn))
      {
        cmd.Parameters.AddWithValue("@name", project.ProjectName);
        cmd.Parameters.AddWithValue("@rate", (double)project.HourlyRate);
        cmd.Parameters.AddWithValue("@status", project.Status);
        cmd.Parameters.AddWithValue("@clientId", project.ClientId);
        cmd.Parameters.AddWithValue("@created", project.CreatedAt.ToString("o"));
        cmd.ExecuteNonQuery();
      }

      using (SqliteCommand lastId = new SqliteCommand("SELECT last_insert_rowid()", conn))
      {
        project.Id = (int)(long)lastId.ExecuteScalar();
      }
    }
  }

  public static List<Project> GetProjects()
  {
    List<Project> projects = new List<Project>();

    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "SELECT Id, ProjectName, HourlyRate, Status, ClientId, CreatedAt FROM Projects ORDER BY Id",
        conn))
      using (SqliteDataReader reader = cmd.ExecuteReader())
      {
        while (reader.Read())
        {
          Project project = new Project(
            reader.GetString(1),
            (decimal)reader.GetDouble(2),
            reader.GetString(3),
            reader.GetInt32(4)
          );
          project.Id = reader.GetInt32(0);
          project.CreatedAt = DateTime.Parse(reader.GetString(5));
          projects.Add(project);
        }
      }
    }

    return projects;
  }

  public static Project GetProjectById(int id)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "SELECT Id, ProjectName, HourlyRate, Status, ClientId, CreatedAt FROM Projects WHERE Id = @id",
        conn))
      {
        cmd.Parameters.AddWithValue("@id", id);

        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
          if (reader.Read())
          {
            Project project = new Project(
              reader.GetString(1),
              (decimal)reader.GetDouble(2),
              reader.GetString(3),
              reader.GetInt32(4)
            );
            project.Id = reader.GetInt32(0);
            project.CreatedAt = DateTime.Parse(reader.GetString(5));
            return project;
          }
        }
      }
    }

    return null;
  }

  public static List<Project> GetProjectsByClient(int clientId)
  {
    List<Project> projects = new List<Project>();

    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "SELECT Id, ProjectName, HourlyRate, Status, ClientId, CreatedAt FROM Projects WHERE ClientId = @clientId ORDER BY Id",
        conn))
      {
        cmd.Parameters.AddWithValue("@clientId", clientId);

        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
          while (reader.Read())
          {
            Project project = new Project(
              reader.GetString(1),
              (decimal)reader.GetDouble(2),
              reader.GetString(3),
              reader.GetInt32(4)
            );
            project.Id = reader.GetInt32(0);
            project.CreatedAt = DateTime.Parse(reader.GetString(5));
            projects.Add(project);
          }
        }
      }
    }

    return projects;
  }

  public static void UpdateProject(Project project)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "UPDATE Projects SET ProjectName = @name, HourlyRate = @rate, Status = @status WHERE Id = @id",
        conn))
      {
        cmd.Parameters.AddWithValue("@name", project.ProjectName);
        cmd.Parameters.AddWithValue("@rate", (double)project.HourlyRate);
        cmd.Parameters.AddWithValue("@status", project.Status);
        cmd.Parameters.AddWithValue("@id", project.Id);
        cmd.ExecuteNonQuery();
      }
    }
  }

  public static void DeleteProject(int projectId)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand("DELETE FROM Projects WHERE Id = @id", conn))
      {
        cmd.Parameters.AddWithValue("@id", projectId);
        cmd.ExecuteNonQuery();
      }
    }
  }

  // ==================== TASK METHODS ====================

  public static void CreateTask(TaskItem task)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "INSERT INTO Tasks (Title, Description, IsCompleted, ProjectId, CreatedAt) VALUES (@title, @desc, @completed, @projectId, @created)",
        conn))
      {
        cmd.Parameters.AddWithValue("@title", task.Title);
        cmd.Parameters.AddWithValue("@desc", task.Description ?? "");
        cmd.Parameters.AddWithValue("@completed", task.IsCompleted ? 1 : 0);
        cmd.Parameters.AddWithValue("@projectId", task.ProjectId);
        cmd.Parameters.AddWithValue("@created", task.CreatedAt.ToString("o"));
        cmd.ExecuteNonQuery();
      }

      using (SqliteCommand lastId = new SqliteCommand("SELECT last_insert_rowid()", conn))
      {
        task.Id = (int)(long)lastId.ExecuteScalar();
      }
    }
  }

  public static List<TaskItem> GetAllTasks()
  {
    List<TaskItem> tasks = new List<TaskItem>();

    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "SELECT Id, Title, Description, IsCompleted, ProjectId, CreatedAt FROM Tasks ORDER BY Id",
        conn))
      using (SqliteDataReader reader = cmd.ExecuteReader())
      {
        while (reader.Read())
        {
          TaskItem task = new TaskItem(
            reader.GetString(1),
            reader.GetString(2),
            reader.GetInt32(4)
          );
          task.Id = reader.GetInt32(0);
          task.IsCompleted = reader.GetInt32(3) == 1;
          task.CreatedAt = DateTime.Parse(reader.GetString(5));
          tasks.Add(task);
        }
      }
    }

    return tasks;
  }

  public static TaskItem GetTaskById(int id)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "SELECT Id, Title, Description, IsCompleted, ProjectId, CreatedAt FROM Tasks WHERE Id = @id",
        conn))
      {
        cmd.Parameters.AddWithValue("@id", id);

        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
          if (reader.Read())
          {
            TaskItem task = new TaskItem(
              reader.GetString(1),
              reader.GetString(2),
              reader.GetInt32(4)
            );
            task.Id = reader.GetInt32(0);
            task.IsCompleted = reader.GetInt32(3) == 1;
            task.CreatedAt = DateTime.Parse(reader.GetString(5));
            return task;
          }
        }
      }
    }

    return null;
  }

  public static List<TaskItem> GetTasksByProject(int projectId)
  {
    List<TaskItem> tasks = new List<TaskItem>();

    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "SELECT Id, Title, Description, IsCompleted, ProjectId, CreatedAt FROM Tasks WHERE ProjectId = @projectId ORDER BY Id",
        conn))
      {
        cmd.Parameters.AddWithValue("@projectId", projectId);

        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
          while (reader.Read())
          {
            TaskItem task = new TaskItem(
              reader.GetString(1),
              reader.GetString(2),
              reader.GetInt32(4)
            );
            task.Id = reader.GetInt32(0);
            task.IsCompleted = reader.GetInt32(3) == 1;
            task.CreatedAt = DateTime.Parse(reader.GetString(5));
            tasks.Add(task);
          }
        }
      }
    }

    return tasks;
  }

  public static void UpdateTask(TaskItem task)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "UPDATE Tasks SET Title = @title, Description = @desc, IsCompleted = @completed WHERE Id = @id",
        conn))
      {
        cmd.Parameters.AddWithValue("@title", task.Title);
        cmd.Parameters.AddWithValue("@desc", task.Description ?? "");
        cmd.Parameters.AddWithValue("@completed", task.IsCompleted ? 1 : 0);
        cmd.Parameters.AddWithValue("@id", task.Id);
        cmd.ExecuteNonQuery();
      }
    }
  }

  public static void DeleteTask(int taskId)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand("DELETE FROM Tasks WHERE Id = @id", conn))
      {
        cmd.Parameters.AddWithValue("@id", taskId);
        cmd.ExecuteNonQuery();
      }
    }
  }

  // ==================== TIME ENTRY METHODS ====================

  public static void CreateTimeEntry(TimeEntry entry)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "INSERT INTO TimeEntries (HoursWorked, HourlyRate, WorkDescription, ProjectId, CreatedAt) VALUES (@hours, @rate, @desc, @projectId, @created)",
        conn))
      {
        cmd.Parameters.AddWithValue("@hours", entry.HoursWorked);
        cmd.Parameters.AddWithValue("@rate", (double)entry.HourlyRate);
        cmd.Parameters.AddWithValue("@desc", entry.WorkDescription ?? "");
        cmd.Parameters.AddWithValue("@projectId", entry.ProjectId);
        cmd.Parameters.AddWithValue("@created", entry.CreatedAt.ToString("o"));
        cmd.ExecuteNonQuery();
      }

      using (SqliteCommand lastId = new SqliteCommand("SELECT last_insert_rowid()", conn))
      {
        entry.Id = (int)(long)lastId.ExecuteScalar();
      }
    }
  }

  public static List<TimeEntry> GetAllTimeEntries()
  {
    List<TimeEntry> entries = new List<TimeEntry>();

    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "SELECT Id, HoursWorked, HourlyRate, WorkDescription, ProjectId, CreatedAt FROM TimeEntries ORDER BY Id",
        conn))
      using (SqliteDataReader reader = cmd.ExecuteReader())
      {
        while (reader.Read())
        {
          TimeEntry entry = new TimeEntry(
            reader.GetDouble(1),
            (decimal)reader.GetDouble(2),
            reader.GetString(3),
            reader.GetInt32(4)
          );
          entry.Id = reader.GetInt32(0);
          entry.CreatedAt = DateTime.Parse(reader.GetString(5));
          entries.Add(entry);
        }
      }
    }

    return entries;
  }

  public static TimeEntry GetTimeEntryById(int id)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "SELECT Id, HoursWorked, HourlyRate, WorkDescription, ProjectId, CreatedAt FROM TimeEntries WHERE Id = @id",
        conn))
      {
        cmd.Parameters.AddWithValue("@id", id);

        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
          if (reader.Read())
          {
            TimeEntry entry = new TimeEntry(
              reader.GetDouble(1),
              (decimal)reader.GetDouble(2),
              reader.GetString(3),
              reader.GetInt32(4)
            );
            entry.Id = reader.GetInt32(0);
            entry.CreatedAt = DateTime.Parse(reader.GetString(5));
            return entry;
          }
        }
      }
    }

    return null;
  }

  public static List<TimeEntry> GetTimeEntriesByProject(int projectId)
  {
    List<TimeEntry> entries = new List<TimeEntry>();

    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "SELECT Id, HoursWorked, HourlyRate, WorkDescription, ProjectId, CreatedAt FROM TimeEntries WHERE ProjectId = @projectId ORDER BY Id",
        conn))
      {
        cmd.Parameters.AddWithValue("@projectId", projectId);

        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
          while (reader.Read())
          {
            TimeEntry entry = new TimeEntry(
              reader.GetDouble(1),
              (decimal)reader.GetDouble(2),
              reader.GetString(3),
              reader.GetInt32(4)
            );
            entry.Id = reader.GetInt32(0);
            entry.CreatedAt = DateTime.Parse(reader.GetString(5));
            entries.Add(entry);
          }
        }
      }
    }

    return entries;
  }

  public static void UpdateTimeEntry(TimeEntry entry)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand(
        "UPDATE TimeEntries SET HoursWorked = @hours, HourlyRate = @rate, WorkDescription = @desc WHERE Id = @id",
        conn))
      {
        cmd.Parameters.AddWithValue("@hours", entry.HoursWorked);
        cmd.Parameters.AddWithValue("@rate", (double)entry.HourlyRate);
        cmd.Parameters.AddWithValue("@desc", entry.WorkDescription ?? "");
        cmd.Parameters.AddWithValue("@id", entry.Id);
        cmd.ExecuteNonQuery();
      }
    }
  }

  public static void DeleteTimeEntry(int entryId)
  {
    using (SqliteConnection conn = new SqliteConnection(ConnectionString))
    {
      conn.Open();

      using (SqliteCommand cmd = new SqliteCommand("DELETE FROM TimeEntries WHERE Id = @id", conn))
      {
        cmd.Parameters.AddWithValue("@id", entryId);
        cmd.ExecuteNonQuery();
      }
    }
  }
}
