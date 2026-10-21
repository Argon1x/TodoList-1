namespace TodoListApp.Models;

public static class EnumDisplayExtensions
{
    public static string ToDisplayString(this Priority priority) => priority switch
    {
        Priority.Low => "Низкий",
        Priority.Medium => "Средний",
        Priority.High => "Высокий",
        _ => priority.ToString()
    };

    public static string ToDisplayString(this TodoStatus status) => status switch
    {
        TodoStatus.NotStarted => "Не начата",
        TodoStatus.InProgress => "В процессе",
        TodoStatus.Completed => "Выполнена",
        _ => status.ToString()
    };
}
