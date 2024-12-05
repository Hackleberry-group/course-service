using System.ComponentModel.DataAnnotations;

namespace CourseServiceAPI.Models.Course.DTOs
{
    public class CourseRequestDTO
    {
        [Required]
        [StringLength(100, MinimumLength=1)]
        public required string Name { get; init; }
        
        [Required]
        public string ProgrammingLanguage {
            get { return ProgrammingLanguage; } 
            set 
            {
                if (Enum.TryParse<ProgrammingLanguage>(value, out var _))
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
        public required Guid TeacherId { get; init; }
    }
}
