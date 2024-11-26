using CourseServiceAPI.Models.Course;
using FluentValidation;

namespace CourseServiceAPI.Validators;

public class CourseValidator : AbstractValidator<Course>
{
    public CourseValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Title is required");
    }
}