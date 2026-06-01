/*
Name: Sherika Fayson
Date: May 31, 2026
Assignment: SDC320 Course Project - Phase 1 Class Implementation
Description: Generates invoice summaries for a client project. Demonstrates composition
             because an Invoice contains a Client, a Project, and a list of TimeEntry
             objects. Outputs a formatted invoice to the terminal.
*/

using System.Collections.Generic;
using System.Text;

public class Invoice
{
  private Client m_client;
  private Project m_project;
  private List<TimeEntry> m_timeEntries;

  public Client Client
  {
    get { return m_client; }
    set { m_client = value; }
  }

  public Project Project
  {
    get { return m_project; }
    set { m_project = value; }
  }

  public List<TimeEntry> TimeEntries
  {
    get { return m_timeEntries; }
    set { m_timeEntries = value; }
  }

  public Invoice(Client client, Project project)
  {
    m_client = client;
    m_project = project;
    m_timeEntries = new List<TimeEntry>();
  }

  public Invoice(Client client, Project project, List<TimeEntry> timeEntries)
  {
    m_client = client;
    m_project = project;
    m_timeEntries = timeEntries;
  }

  public decimal CalculateInvoiceTotal()
  {
    decimal total = 0;
    foreach (TimeEntry entry in m_timeEntries)
    {
      total += entry.CalculateTotal();
    }
    return total;
  }

  public override string ToString()
  {
    return GenerateInvoiceSummary();
  }

  public string GenerateInvoiceSummary()
  {
    StringBuilder sb = new StringBuilder();

    sb.AppendLine("==================================================");
    sb.AppendLine("              INVOICE SUMMARY");
    sb.AppendLine("==================================================");
    sb.AppendLine($"Client:   {m_client.Name}");
    sb.AppendLine($"Email:    {m_client.Email}");
    sb.AppendLine($"Phone:    {m_client.PhoneNumber}");
    sb.AppendLine("--------------------------------------------------");
    sb.AppendLine($"Project:  {m_project.ProjectName}");
    sb.AppendLine($"Status:   {m_project.Status}");
    sb.AppendLine("--------------------------------------------------");
    sb.AppendLine("Time Entries:");

    if (m_timeEntries.Count == 0)
    {
      sb.AppendLine("  No time entries recorded.");
    }
    else
    {
      foreach (TimeEntry entry in m_timeEntries)
      {
        sb.AppendLine($"  - {entry.WorkDescription}");
        sb.AppendLine($"    {entry.HoursWorked} hrs @ ${entry.HourlyRate:F2}/hr = ${entry.CalculateTotal():F2}");
      }
    }

    sb.AppendLine("--------------------------------------------------");
    sb.AppendLine($"INVOICE TOTAL:  ${CalculateInvoiceTotal():F2}");
    sb.AppendLine("==================================================");

    return sb.ToString();
  }
}
