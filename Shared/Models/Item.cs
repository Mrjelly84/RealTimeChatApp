using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class Item
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
    }
}