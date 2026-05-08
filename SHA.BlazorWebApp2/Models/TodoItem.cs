using System.ComponentModel.DataAnnotations;

namespace SHA.BlazorWebApp2.Models;

public class TodoItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public bool IsCompleted { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
