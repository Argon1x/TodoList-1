using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using TodoListApp.Models;
using TodoListApp.Repositories;
using TodoListApp.Services;

namespace TodoListApp;

public partial class MainWindow : Window
{
    private readonly TodoService _todoService;
    private TodoModel? _selectedTask;

    public MainWindow()
    {
        InitializeComponent();

        string filePath = Path.Combine(AppContext.BaseDirectory, "tasks.json");
        _todoService = new TodoService(new JsonTaskRepository(filePath));

        PriorityComboBox.ItemsSource = Enum.GetValues<Priority>();
        StatusComboBox.ItemsSource = Enum.GetValues<TodoStatus>();

        PriorityComboBox.ItemTemplate = new FuncDataTemplate<Priority>(
            (value, _) => new TextBlock { Text = value.ToDisplayString() });
        StatusComboBox.ItemTemplate = new FuncDataTemplate<TodoStatus>(
            (value, _) => new TextBlock { Text = value.ToDisplayString() });

        PriorityComboBox.SelectedIndex = 0;
        StatusComboBox.SelectedIndex = 0;

        RefreshTasksList();
    }

    private void RefreshTasksList()
    {
        TasksListBox.ItemsSource = null;
        TasksListBox.ItemsSource = _todoService.GetAll();
    }

    private void TasksListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selectedTask = TasksListBox.SelectedItem as TodoModel;

        if (_selectedTask is null)
            return;

        TitleTextBox.Text = _selectedTask.Title;
        DescriptionTextBox.Text = _selectedTask.Description;
        CreatedAtTextBlock.Text = _selectedTask.CreatedAt.ToString("dd.MM.yyyy HH:mm");
        DueDatePicker.SelectedDate = _selectedTask.DueDate;
        PriorityComboBox.SelectedItem = _selectedTask.Priority;
        StatusComboBox.SelectedItem = _selectedTask.Status;
        ErrorTextBlock.Text = string.Empty;
    }

    private void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            DateTime dueDate = DueDatePicker.SelectedDate?.DateTime ?? DateTime.Now;
            var priority = (Priority)(PriorityComboBox.SelectedItem ?? Priority.Low);

            _todoService.Add(TitleTextBox.Text ?? string.Empty, DescriptionTextBox.Text ?? string.Empty, dueDate, priority);

            RefreshTasksList();
            ClearForm();
        }
        catch (ArgumentException ex)
        {
            ErrorTextBlock.Text = ex.Message;
        }
    }

    private void EditButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_selectedTask is null)
        {
            ErrorTextBlock.Text = "Сначала выбери задачу в списке слева.";
            return;
        }

        try
        {
            DateTime dueDate = DueDatePicker.SelectedDate?.DateTime ?? _selectedTask.DueDate;
            var priority = (Priority)(PriorityComboBox.SelectedItem ?? _selectedTask.Priority);
            var status = (TodoStatus)(StatusComboBox.SelectedItem ?? _selectedTask.Status);

            _todoService.Edit(_selectedTask.Id, TitleTextBox.Text ?? string.Empty, DescriptionTextBox.Text ?? string.Empty, dueDate, priority);
            _todoService.ChangeStatus(_selectedTask.Id, status);

            RefreshTasksList();
            ErrorTextBlock.Text = string.Empty;
        }
        catch (ArgumentException ex)
        {
            ErrorTextBlock.Text = ex.Message;
        }
    }

    private void DeleteButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_selectedTask is null)
        {
            ErrorTextBlock.Text = "Сначала выбери задачу в списке слева.";
            return;
        }

        _todoService.Delete(_selectedTask.Id);
        _selectedTask = null;

        RefreshTasksList();
        ClearForm();
    }

    private void ClearButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _selectedTask = null;
        TasksListBox.SelectedItem = null;
        ClearForm();
    }

    private void ClearForm()
    {
        TitleTextBox.Text = string.Empty;
        DescriptionTextBox.Text = string.Empty;
        CreatedAtTextBlock.Text = "(появится после выбора задачи в списке)";
        DueDatePicker.SelectedDate = null;
        PriorityComboBox.SelectedIndex = 0;
        StatusComboBox.SelectedIndex = 0;
        ErrorTextBlock.Text = string.Empty;
    }
}
