using System.ComponentModel.DataAnnotations;

namespace CourseServiceAPI.Models.Course.DTOs
{
    public class CourseRequestDTO
    {
        [Required]
        public required string Name { get; init; }
        [Required]
        public string ProgrammingLanguage {
            get { return ProgrammingLanguage; } 
            set 
            {
                if (Enum.TryParse<ProgrammingLanguage>(value, out var language))
                {
                    ProgrammingLanguage = value;
                }
                else
                {
                    throw new ArgumentException("Invalid Programming Language");
                }
            } 
        }
        [Required]
        public required string TeacherId { get; init; }
    }
}
