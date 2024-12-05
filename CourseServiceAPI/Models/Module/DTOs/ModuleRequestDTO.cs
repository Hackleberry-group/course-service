using System.ComponentModel.DataAnnotations;

namespace CourseServiceAPI.Models.Module.DTOs
{
    public record ModuleRequestDTO
    {
        [Required]
        public string Name { get; init; }
        
        [Required]
        public Guid CourseId { get; init; }
        
        [Required]
        public int Order { get; init; }
    }
}
