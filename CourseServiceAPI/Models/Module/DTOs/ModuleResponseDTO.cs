using System.ComponentModel.DataAnnotations;

namespace CourseServiceAPI.Models.Module.DTOs
{
    public class ModuleResponseDTO
    {
        [Required]
        public Guid Id { get; init; }

        [Required]
        public string Name { get; init; }

        [Required]
        public Guid CourseId { get; init; }

        [Required]
        public int Order { get; init; }
    }
}
