using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Course;
using CourseServiceAPI.Services;

namespace CourseServiceAPI.Tests.CourseServiceTests;

[TestFixture]
public class CourseServiceTests
{
    private ICourseService _courseService;

    [SetUp]
    public void Setup(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [Test]
    public async Task GetCourses_ShouldReturnListOfCourses()
    {
        var result = await _courseService.GetCoursesAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Exactly(3).Items);
    }

    [Test]
    public async Task CreateCourse_ShouldReturnCreatedCourse()
    {
        var course = new Course { RowKey = Guid.NewGuid().ToString(), Name = "New Course" };

        var result = await _courseService.CreateCourseAsync(course);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RowKey, Is.EqualTo(course.RowKey));
            Assert.That(result.Name, Is.EqualTo(course.Name));
        });
    }
}