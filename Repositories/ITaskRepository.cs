using System.Collections.Generic;
using TodoListApp.Models;

namespace TodoListApp.Repositories;

public interface ITaskRepository
{
    List<TodoModel> Load();

    void Save(List<TodoModel> tasks);
}
