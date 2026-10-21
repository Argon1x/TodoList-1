using System;
using System.Collections.Generic;
using System.Linq;
using TodoListApp.Models;
using TodoListApp.Repositories;

namespace TodoListApp.Services;

public class TodoService
{
    private readonly ITaskRepository _repository;
    private readonly List<TodoModel> _tasks;

    public TodoService(ITaskRepository repository)
    {
        _repository = repository;
        _tasks = _repository.Load();
    }

    public IReadOnlyList<TodoModel> GetAll() => _tasks;

    public TodoModel Add(string title, string description, DateTime dueDate, Priority priority)
    {
        var task = new TodoModel(title, description, dueDate, priority);
        _tasks.Add(task);
        _repository.Save(_tasks);
        return task;
    }

    public void Edit(Guid id, string title, string description, DateTime dueDate, Priority priority)
    {
        TodoModel task = FindTaskOrThrow(id);

        task.Title = title;
        task.Description = description;
        task.DueDate = dueDate;
        task.Priority = priority;

        _repository.Save(_tasks);
    }

    public void ChangeStatus(Guid id, TodoStatus status)
    {
        TodoModel task = FindTaskOrThrow(id);
        task.Status = status;
        _repository.Save(_tasks);
    }

    public void Delete(Guid id)
    {
        TodoModel task = FindTaskOrThrow(id);
        _tasks.Remove(task);
        _repository.Save(_tasks);
    }

    private TodoModel FindTaskOrThrow(Guid id)
    {
        TodoModel? task = _tasks.FirstOrDefault(t => t.Id == id);

        if (task is null)
            throw new InvalidOperationException("Задача с таким идентификатором не найдена.");

        return task;
    }
}
