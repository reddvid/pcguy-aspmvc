using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCGuy.Common.Enums;

namespace PCGuy.Common.Entities;

public class Category
{
    [Key] public int Id { get; init; }
    [Required] public string? Name { get; init; }
}