using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;
using FluentValidation;

namespace CourseServiceAPI.Validators
{
    public class ExerciseDtoValidator : AbstractValidator<ExerciseRequestDTO>
    {
        public ExerciseDtoValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty().WithMessage("Topic ID is required.")
                .Must(BeAValidGuid).WithMessage("Topic ID must be a valid GUID.");

            RuleFor(x => x.Order)
                .GreaterThan(0).WithMessage("Order must be greater than 0.");

            RuleFor(x => x.IsTopicExam)
                .NotNull().WithMessage("IsTopicExam must be specified.");
        }

        private bool BeAValidGuid(string value)
        {
            return Guid.TryParse(value, out _);
        }
    }
}
