using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TodoListApp.Models;

namespace TodoListApp.Repositories;

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    public JsonTaskRepository(string filePath)
    {
        _filePath = filePath;
    }

    public List<TodoModel> Load()
    {
        if (!File.Exists(_filePath))
            return new List<TodoModel>();

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
            return new List<TodoModel>();

        List<TodoModel>? tasks = JsonSerializer.Deserialize<List<TodoModel>>(json, SerializerOptions);

        return tasks ?? new List<TodoModel>();
    }

    public void Save(List<TodoModel> tasks)
    {
        string json = JsonSerializer.Serialize(tasks, SerializerOptions);
        File.WriteAllText(_filePath, json);
    }
}
