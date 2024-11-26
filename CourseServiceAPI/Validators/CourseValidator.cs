using CourseServiceAPI.Models.Course;
using FluentValidation;

namespace CourseServiceAPI.Validators;

public class CourseValidator : AbstractValidator<Course>
{
    public CourseValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
    }
}