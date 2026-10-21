using System;
using System.Text.Json.Serialization;

namespace TodoListApp.Models;

public enum Priority
{
    Low,
    Medium,
    High
}

public enum TodoStatus
{
    NotStarted,
    InProgress,
    Completed
}

public class TodoModel
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private DateTime _dueDate;

    public Guid Id { get; }
    public DateTime CreatedAt { get; }

    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Название задачи не может быть пустым.");

            _title = value;
        }
    }

    public string Description
    {
        get => _description;
        set => _description = value ?? string.Empty;
    }

    public DateTime DueDate
    {
        get => _dueDate;
        set
        {
            if (value.Date < CreatedAt.Date)
                throw new ArgumentException("Срок выполнения не может быть раньше даты создания задачи.");

            _dueDate = value;
        }
    }

    public Priority Priority { get; set; }

    public TodoStatus Status { get; set; }

    public TodoModel(string title, string description, DateTime dueDate, Priority priority)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;

        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        Status = TodoStatus.NotStarted;
    }

    [JsonConstructor]
    public TodoModel(Guid id, string title, string description, DateTime createdAt, DateTime dueDate, Priority priority, TodoStatus status)
    {
        Id = id;
        CreatedAt = createdAt;

        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        Status = status;
    }

    public override string ToString()
    {
        return $"[{Status.ToDisplayString()}] {Title} " +
               $"(приоритет: {Priority.ToDisplayString()}, " +
               $"срок: {DueDate:dd.MM.yyyy}, создана: {CreatedAt:dd.MM.yyyy})";
    }
}
