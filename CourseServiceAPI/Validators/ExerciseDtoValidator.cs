using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;
using FluentValidation;

namespace CourseServiceAPI.Validators
{
    public class ExerciseDtoValidator : AbstractValidator<ExerciseDto>
    {
        public ExerciseDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Exercise ID is required.")
                .Must(BeAValidGuid).WithMessage("Exercise ID must be a valid GUID.");

            RuleFor(x => x.TopicId)
                .NotEmpty().WithMessage("Topic ID is required.")
                .Must(BeAValidGuid).WithMessage("Topic ID must be a valid GUID.");

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
            RuleFor(completion => completion.ExerciseId).NotEmpty().WithMessage("Exercise ID is required.");
            RuleForEach(completion => completion.AnsweredQuestions).SetValidator(new AnsweredQuestionListValidator());
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
            RuleFor(answeredQuestion => answeredQuestion.QuestionId).NotEmpty().WithMessage("Question ID is required.");
            RuleFor(answeredQuestion => answeredQuestion.AnswerId).NotEmpty().WithMessage("Answer ID is required.");
        }
    }
}