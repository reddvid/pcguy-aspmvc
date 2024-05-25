using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCGuy.Entities.Entities;

public class Brand
{
    [Key]
    public int Id { get; set; }
    [Required] public string? Name { get; set; }
}