/*
Name: Sherika Fayson
Date: May 31, 2026
Assignment: SDC320 Course Project - Phase 1 Class Implementation
Description: Represents a client with contact information. Inherits from BaseEntity
             and demonstrates polymorphism by overriding GetInfo(). Uses ToString()
             for formatted terminal output.
*/

public class Client : BaseEntity
{
  private string m_name;
  private string m_email;
  private string m_phoneNumber;

  public string Name
  {
    get { return m_name; }
    set { m_name = value; }
  }

  public string Email
  {
    get { return m_email; }
    set { m_email = value; }
  }

  public string PhoneNumber
  {
    get { return m_phoneNumber; }
    set { m_phoneNumber = value; }
  }

  public Client(string name, string email, string phoneNumber)
  {
    m_name = name;
    m_email = email;
    m_phoneNumber = phoneNumber;
  }

  public override string GetInfo()
  {
    return $"Client [{Id}]: {m_name} | {m_email} | {m_phoneNumber}";
  }

  public override string ToString()
  {
    return $"Client ID:  {Id}\n" +
           $"Name:       {m_name}\n" +
           $"Email:      {m_email}\n" +
           $"Phone:      {m_phoneNumber}\n" +
           $"Created:    {CreatedAt:MM/dd/yyyy}";
  }
}
