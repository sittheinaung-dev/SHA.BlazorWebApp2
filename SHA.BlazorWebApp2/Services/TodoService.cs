using System.Text.Json;
using Microsoft.JSInterop;
using SHA.BlazorWebApp2.Models;

namespace SHA.BlazorWebApp2.Services;

public class TodoService
{
    private readonly IJSRuntime _js;
    private const string StorageKey = "todo_items";
    private List<TodoItem> _items = new();

    public TodoService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<List<TodoItem>> GetItemsAsync()
    {
        try
        {
            var json = await _js.InvokeAsync<string>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(json))
            {
                _items = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new();
            }
        }
        catch
        {
            // Fallback to in-memory if localStorage fails
        }
        return _items.OrderByDescending(i => i.CreatedAt).ToList();
    }

    public async Task AddItemAsync(TodoItem item)
    {
        _items.Add(item);
        await SaveToStorageAsync();
    }

    public async Task UpdateItemAsync(TodoItem item)
    {
        var existing = _items.FirstOrDefault(i => i.Id == item.Id);
        if (existing != null)
        {
            existing.Title = item.Title;
            existing.Description = item.Description;
            existing.IsCompleted = item.IsCompleted;
            await SaveToStorageAsync();
        }
    }

    public async Task DeleteItemAsync(Guid id)
    {
        _items.RemoveAll(i => i.Id == id);
        await SaveToStorageAsync();
    }

    private async Task SaveToStorageAsync()
    {
        var json = JsonSerializer.Serialize(_items);
        await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }
}
