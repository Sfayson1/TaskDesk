/*
Name: Sherika Fayson
Date: May 31, 2026
Assignment: SDC320 Course Project - Phase 1 Class Implementation
Description: Abstract base class that provides shared entity properties (Id, CreatedAt)
             for all model classes. Demonstrates abstraction and is the parent for
             Client, Project, TaskItem, and TimeEntry through inheritance.
*/

using System;

public abstract class BaseEntity
{
  private int m_id;
  private DateTime m_createdAt;

  public int Id
  {
    get { return m_id; }
    set { m_id = value; }
  }

  public DateTime CreatedAt
  {
    get { return m_createdAt; }
    set { m_createdAt = value; }
  }

  protected BaseEntity()
  {
    m_id = 0;
    m_createdAt = DateTime.Now;
  }

  public abstract string GetInfo();

  public override string ToString()
  {
    return GetInfo();
  }
}
