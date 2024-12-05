using CourseServiceAPI.Models;
using CourseServiceAPI.Models.Course;
using CourseServiceAPI.Validators;
using FluentValidation.TestHelper;

namespace CourseServiceAPI.Tests.ValidatorTests;

[TestFixture]
public class CourseValidatorTests
{
    private CourseValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new CourseValidator();
    }

    [Test]
    public void ShouldHaveErrorWhenNameIsEmpty()
    {
        var course = new Course { Name = string.Empty };

        var result = _validator.TestValidate(course);

        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Test]
    public void ShouldNotHaveErrorWhenNameIsSpecified()
    {
        var course = new Course { Name = "Valid Title" };

        var result = _validator.TestValidate(course);

        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }
}