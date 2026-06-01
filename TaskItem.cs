/*
Name: Sherika Fayson
Date: May 31, 2026
Assignment: SDC320 Course Project - Phase 1 Class Implementation
Description: Represents a task associated with a project. Inherits from BaseEntity.
             Provides MarkComplete() to update task status and overrides GetInfo()
             to output task status information to the terminal.
*/

public class TaskItem : BaseEntity
{
  private string m_title;
  private string m_description;
  private bool m_isCompleted;
  private int m_projectId;

  public string Title
  {
    get { return m_title; }
    set { m_title = value; }
  }

  public string Description
  {
    get { return m_description; }
    set { m_description = value; }
  }

  public bool IsCompleted
  {
    get { return m_isCompleted; }
    set { m_isCompleted = value; }
  }

  public int ProjectId
  {
    get { return m_projectId; }
    set { m_projectId = value; }
  }

  public TaskItem(string title, string description, int projectId)
  {
    m_title = title;
    m_description = description;
    m_isCompleted = false;
    m_projectId = projectId;
  }

  public void MarkComplete()
  {
    m_isCompleted = true;
  }

  public override string GetInfo()
  {
    string status = m_isCompleted ? "Completed" : "Pending";
    return $"Task [{Id}]: {m_title} | Status: {status} | Project ID: {m_projectId}";
  }

  public override string ToString()
  {
    string status = m_isCompleted ? "Completed" : "Pending";
    return $"Task ID:      {Id}\n" +
           $"Title:        {m_title}\n" +
           $"Description:  {m_description}\n" +
           $"Status:       {status}\n" +
           $"Project ID:   {m_projectId}\n" +
           $"Created:      {CreatedAt:MM/dd/yyyy}";
  }
}
