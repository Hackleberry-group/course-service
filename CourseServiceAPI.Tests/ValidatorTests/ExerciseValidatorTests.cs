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
        private ExerciseRequestDTO _validExerciseRequest;

        [SetUp]
        public void Setup()
        {
            _exerciseDtoValidator = new ExerciseDtoValidator();

            _validExerciseRequest = new ExerciseRequestDTO
            {
                TopicId = Guid.NewGuid().ToString(),
                Order = 1,
                IsTopicExam = true
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
    }
}

