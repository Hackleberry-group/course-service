using CourseServiceAPI.Models.Course;

namespace CourseServiceAPI.Interfaces;

public interface ICourseService
{
    IEnumerable<Course> GetCourses();

    Course CreateCourse(Course course);
}