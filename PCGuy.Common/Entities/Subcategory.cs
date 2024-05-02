using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCGuy.Common.Enums;

namespace PCGuy.Common.Entities;

public class Subcategory
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}