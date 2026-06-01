/*
Name: Sherika Fayson
Date: May 31, 2026
Assignment: SDC320 Course Project - Phase 1 Class Implementation
Description: Represents a project assigned to a client. Inherits from BaseEntity and
             demonstrates composition (projects belong to clients via ClientId) and
             polymorphism by overriding GetInfo().
*/

public class Project : BaseEntity
{
  private string m_projectName;
  private decimal m_hourlyRate;
  private string m_status;
  private int m_clientId;

  public string ProjectName
  {
    get { return m_projectName; }
    set { m_projectName = value; }
  }

  public decimal HourlyRate
  {
    get { return m_hourlyRate; }
    set { m_hourlyRate = value; }
  }

  public string Status
  {
    get { return m_status; }
    set { m_status = value; }
  }

  public int ClientId
  {
    get { return m_clientId; }
    set { m_clientId = value; }
  }

  public Project(string projectName, decimal hourlyRate, string status, int clientId)
  {
    m_projectName = projectName;
    m_hourlyRate = hourlyRate;
    m_status = status;
    m_clientId = clientId;
  }

  public override string GetInfo()
  {
    return $"Project [{Id}]: {m_projectName} | Rate: ${m_hourlyRate:F2}/hr | Status: {m_status} | Client ID: {m_clientId}";
  }

  public override string ToString()
  {
    return $"Project ID:   {Id}\n" +
           $"Name:         {m_projectName}\n" +
           $"Hourly Rate:  ${m_hourlyRate:F2}\n" +
           $"Status:       {m_status}\n" +
           $"Client ID:    {m_clientId}\n" +
           $"Created:      {CreatedAt:MM/dd/yyyy}";
  }
}
