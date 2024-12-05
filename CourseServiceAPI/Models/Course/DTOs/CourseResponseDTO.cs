using System.ComponentModel.DataAnnotations;

namespace CourseServiceAPI.Models.Course.DTOs
{
    public class CourseResponseDTO
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string Name { get; init; }
        [Required]
        public string ProgrammingLanguage { get; init; }
        [Required]
        public string TeacherId { get; init; }
        [Required]
        public List<Guid> ModuleIds { get; init; } = new List<Guid>();
    }
}
