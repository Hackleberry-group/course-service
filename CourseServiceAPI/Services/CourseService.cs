using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models;

namespace CourseServiceAPI.Services;

public class CourseService : ICourseService
{
    public IEnumerable<Course> GetCourses()
    {
        throw new NotImplementedException();
    }

    public Course CreateCourse(Course course)
    {
        // Logic to create a course can be added here
        return course;
    }
}