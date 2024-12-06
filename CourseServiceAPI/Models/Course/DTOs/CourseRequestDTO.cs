using System.ComponentModel.DataAnnotations;

namespace CourseServiceAPI.Models.Course.DTOs
{
    public class CourseRequestDTO
    {
        [Required]
        [StringLength(100, MinimumLength=1)]
        public required string Name { get; init; }
        
        [Required]
        public string ProgrammingLanguage { get; init; }

        [Required]
        public required Guid TeacherId { get; init; }
    }
}
