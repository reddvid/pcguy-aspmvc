using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCGuy.Entities.Entities;

public class Category
{
    [Key] public int Id { get; init; }
    [Required] public string? Name { get; init; }
}