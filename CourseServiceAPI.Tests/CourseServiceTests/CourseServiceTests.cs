using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Course;
using CourseServiceAPI.Services;

namespace CourseServiceAPI.Tests.CourseServiceTests;

[TestFixture]
public class CourseServiceTests
{
    private ICourseService _courseService;

    [SetUp]
    public void Setup()
    {
        _courseService = new CourseService();
    }

    [Test]
    public void GetCourses_ShouldReturnListOfCourses()
    {
        var result = _courseService.GetCourses();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Exactly(3).Items);
    }

    [Test]
    public void CreateCourse_ShouldReturnCreatedCourse()
    {
        var course = new Course { RowKey = Guid.NewGuid().ToString(), Name = "New Course" };

        var result = _courseService.CreateCourse(course);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RowKey, Is.EqualTo(course.RowKey));
            Assert.That(result.Name, Is.EqualTo(course.Name));
        });
    }
}