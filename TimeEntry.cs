/*
Name: Sherika Fayson
Date: May 31, 2026
Assignment: SDC320 Course Project - Phase 1 Class Implementation
Description: Stores billable work hours for a project. Inherits from BaseEntity and
             implements the IBillable interface. Demonstrates polymorphism through
             interface implementation and GetInfo() override.
*/

public class TimeEntry : BaseEntity, IBillable
{
  private double m_hoursWorked;
  private decimal m_hourlyRate;
  private string m_workDescription;
  private int m_projectId;

  public double HoursWorked
  {
    get { return m_hoursWorked; }
    set { m_hoursWorked = value; }
  }

  public decimal HourlyRate
  {
    get { return m_hourlyRate; }
    set { m_hourlyRate = value; }
  }

  public string WorkDescription
  {
    get { return m_workDescription; }
    set { m_workDescription = value; }
  }

  public int ProjectId
  {
    get { return m_projectId; }
    set { m_projectId = value; }
  }

  public TimeEntry(double hoursWorked, decimal hourlyRate, string workDescription, int projectId)
  {
    m_hoursWorked = hoursWorked;
    m_hourlyRate = hourlyRate;
    m_workDescription = workDescription;
    m_projectId = projectId;
  }

  public decimal CalculateTotal()
  {
    return (decimal)m_hoursWorked * m_hourlyRate;
  }

  public override string GetInfo()
  {
    return $"Time Entry [{Id}]: {m_hoursWorked} hrs @ ${m_hourlyRate:F2}/hr = ${CalculateTotal():F2} | {m_workDescription}";
  }

  public override string ToString()
  {
    return $"Entry ID:     {Id}\n" +
           $"Hours:        {m_hoursWorked}\n" +
           $"Rate:         ${m_hourlyRate:F2}/hr\n" +
           $"Total:        ${CalculateTotal():F2}\n" +
           $"Description:  {m_workDescription}\n" +
           $"Project ID:   {m_projectId}\n" +
           $"Created:      {CreatedAt:MM/dd/yyyy}";
  }
}
