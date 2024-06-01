using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCGuy.Models.Entities;

public class Brand
{
    [Key]
    public int Id { get; set; }
    [Required] public string? Name { get; set; }
}