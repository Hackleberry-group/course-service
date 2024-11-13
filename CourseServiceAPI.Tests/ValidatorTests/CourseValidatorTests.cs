using CourseServiceAPI.Models;
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
    public void ShouldHaveErrorWhenTitleIsEmpty()
    {
        var course = new Course { Title = string.Empty };

        var result = _validator.TestValidate(course);

        result.ShouldHaveValidationErrorFor(c => c.Title);
    }

    [Test]
    public void ShouldNotHaveErrorWhenTitleIsSpecified()
    {
        var course = new Course { Title = "Valid Title" };

        var result = _validator.TestValidate(course);

        result.ShouldNotHaveValidationErrorFor(c => c.Title);
    }
}