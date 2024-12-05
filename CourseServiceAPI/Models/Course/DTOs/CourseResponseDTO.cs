namespace CourseServiceAPI.Models.Course.DTOs
{
    public class CourseResponseDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string ProgrammingLanguage { get; init; }
        public string TeacherId { get; init; }
    }
}
