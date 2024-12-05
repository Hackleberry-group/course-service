using System.ComponentModel.DataAnnotations;

namespace CourseServiceAPI.Models.Course.DTOs
{
    public class CourseRequestDTO
    {
        [Required]
        public string Name { get; init; }
        [Required]
        public string ProgrammingLanguage { get; init; }
        [Required]
        public string TeacherId { get; init; }
    }
}
