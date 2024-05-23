using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCGuy.Common.Enums;

namespace PCGuy.Common.Entities;

public class Subcategory
{
    [Key] public int Id { get; init; }
    [Required] public string? Name { get; init; }

    public int CategoryId { get; init; }
    [ForeignKey("CategoryId")] public Category? Category { get; init; }
}