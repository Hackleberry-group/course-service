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

    public class ExerciseCompletionValidator : AbstractValidator<ExerciseCompletion>
    {
        public ExerciseCompletionValidator()
        {
            RuleFor(completion => completion.ExerciseId)
                .NotEmpty().WithMessage("Exercise ID is required.")
                .Must(BeAValidGuid).WithMessage("Exercise ID must be a valid GUID.");

            RuleFor(completion => completion.AnsweredQuestions)
                .NotEmpty().WithMessage("Answered questions are required.")
                .Must(ContainValidAnsweredQuestions).WithMessage("Answered questions must contain valid entries.");

            RuleForEach(completion => completion.AnsweredQuestions)
                .SetValidator(new AnsweredQuestionListValidator());
        }

        private bool BeAValidGuid(Guid value)
        {
            return value != Guid.Empty;
        }

        private bool ContainValidAnsweredQuestions(List<List<AnsweredQuestion>> answeredQuestions)
        {
            return answeredQuestions != null && answeredQuestions.All(list => list != null && list.Any());
        }
    }

    public class AnsweredQuestionListValidator : AbstractValidator<List<AnsweredQuestion>>
    {
        public AnsweredQuestionListValidator()
        {
            RuleForEach(list => list).SetValidator(new AnsweredQuestionValidator());
        }
    }

    public class AnsweredQuestionValidator : AbstractValidator<AnsweredQuestion>
    {
        public AnsweredQuestionValidator()
        {
            RuleFor(answeredQuestion => answeredQuestion.QuestionId)
                .NotEmpty().WithMessage("Question ID is required.")
                .Must(BeAValidGuid).WithMessage("Question ID must be a valid GUID.");

            RuleFor(answeredQuestion => answeredQuestion.AnswerId)
                .NotEmpty().WithMessage("Answer ID is required.")
                .Must(BeAValidGuid).WithMessage("Answer ID must be a valid GUID.");
        }

        private bool BeAValidGuid(Guid value)
        {
            return value != Guid.Empty;
        }
    }
}
