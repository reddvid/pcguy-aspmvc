using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCGuy.Common.Enums;

namespace PCGuy.Common.Entities;

public class Brand
{
    public int Id { get; set; }
    public string? Name { get; set; }
}