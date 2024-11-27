using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;
using CourseServiceAPI.Validators;
using FluentValidation.TestHelper;

namespace CourseServiceAPI.Tests.ValidatorTests
{
    [TestFixture]
    public class ExerciseValidatorTests
    {
        private ExerciseDtoValidator _exerciseDtoValidator;
        private ExerciseCompletionValidator _exerciseCompletionValidator;
        private ExerciseRequestDTO _validExerciseRequest;
        private ExerciseCompletion _validExerciseCompletion;

        [SetUp]
        public void Setup()
        {
            _exerciseDtoValidator = new ExerciseDtoValidator();
            _exerciseCompletionValidator = new ExerciseCompletionValidator();

            _validExerciseRequest = new ExerciseRequestDTO
            {
                TopicId = Guid.NewGuid().ToString(),
                Order = 1,
                IsTopicExam = true
            };

            _validExerciseCompletion = new ExerciseCompletion
            {
                ExerciseId = Guid.NewGuid(),
                AnsweredQuestions = new List<List<AnsweredQuestion>>
                {
                    new()
                    {
                        new AnsweredQuestion { QuestionId = Guid.NewGuid(), AnswerId = Guid.NewGuid() }
                    }
                }
            };
        }

        [Test]
        public void ShouldHaveErrorWhenPropertiesAreEmptyInExerciseRequest()
        {
            var invalidExerciseRequest = new ExerciseRequestDTO
            {
                TopicId = "",
                Order = 0,
                IsTopicExam = null
            };

            var result = _exerciseDtoValidator.TestValidate(invalidExerciseRequest);

            result.ShouldHaveValidationErrorFor(e => e.TopicId);
            result.ShouldHaveValidationErrorFor(e => e.Order);
            result.ShouldHaveValidationErrorFor(e => e.IsTopicExam);
        }

        [Test]
        public void ShouldNotHaveErrorWhenPropertiesAreSpecifiedInExerciseRequest()
        {
            var result = _exerciseDtoValidator.TestValidate(_validExerciseRequest);

            result.ShouldNotHaveValidationErrorFor(e => e.TopicId);
            result.ShouldNotHaveValidationErrorFor(e => e.Order);
            result.ShouldNotHaveValidationErrorFor(e => e.IsTopicExam);
        }

        [Test]
        public void ShouldHaveErrorWhenPropertiesAreEmptyInExerciseCompletion()
        {
            var invalidExerciseCompletion = new ExerciseCompletion
            {
                ExerciseId = Guid.Empty,
                AnsweredQuestions = new List<List<AnsweredQuestion>>()
            };

            var result = _exerciseCompletionValidator.TestValidate(invalidExerciseCompletion);

            result.ShouldHaveValidationErrorFor(e => e.ExerciseId);
            result.ShouldHaveValidationErrorFor(e => e.AnsweredQuestions);
        }

        [Test]
        public void ShouldNotHaveErrorWhenPropertiesAreSpecifiedInExerciseCompletion()
        {
            var result = _exerciseCompletionValidator.TestValidate(_validExerciseCompletion);

            result.ShouldNotHaveValidationErrorFor(e => e.ExerciseId);
            result.ShouldNotHaveValidationErrorFor(e => e.AnsweredQuestions);
        }

        [Test]
        public void ShouldHaveErrorWhenAnsweredQuestionPropertiesAreEmpty()
        {
            var invalidAnsweredQuestion = new AnsweredQuestion
            {
                QuestionId = Guid.Empty,
                AnswerId = Guid.Empty
            };

            var answeredQuestionValidator = new AnsweredQuestionValidator();
            var result = answeredQuestionValidator.TestValidate(invalidAnsweredQuestion);

            result.ShouldHaveValidationErrorFor(e => e.QuestionId);
            result.ShouldHaveValidationErrorFor(e => e.AnswerId);
        }

        [Test]
        public void ShouldNotHaveErrorWhenAnsweredQuestionPropertiesAreSpecified()
        {
            var validAnsweredQuestion = new AnsweredQuestion
            {
                QuestionId = Guid.NewGuid(),
                AnswerId = Guid.NewGuid()
            };

            var answeredQuestionValidator = new AnsweredQuestionValidator();
            var result = answeredQuestionValidator.TestValidate(validAnsweredQuestion);

            result.ShouldNotHaveValidationErrorFor(e => e.QuestionId);
            result.ShouldNotHaveValidationErrorFor(e => e.AnswerId);
        }
    }
}

