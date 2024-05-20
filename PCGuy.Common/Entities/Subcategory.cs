using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCGuy.Common.Enums;

namespace PCGuy.Common.Entities;

public class Subcategory
{
    [Key] public int Id { get; init; }
    public string? Name { get; init; }

    [ForeignKey("CategoryId")]
    [DisplayName("Category")]
    public int CategoryId { get; init; }

    public Category? Category { get; init; }
}