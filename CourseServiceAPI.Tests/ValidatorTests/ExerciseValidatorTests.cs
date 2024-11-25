using CourseServiceAPI.Models.Exercise.DTOs;
using CourseServiceAPI.Validators;
using FluentValidation.TestHelper;

namespace CourseServiceAPI.Tests.ValidatorTests
{
    [TestFixture]
    public class ExerciseValidatorTests
    {
        private ExerciseDtoValidator _validator;
        private ExerciseRequestDTO _validExercise;

        [SetUp]
        public void Setup()
        {
            _validator = new ExerciseDtoValidator();

            _validExercise = new ExerciseRequestDTO
            {
                TopicId = Guid.NewGuid().ToString(),
                IsTopicExam = true
            };
        }

        [Test]
        public void ShouldHaveErrorWhenPropertiesAreEmpty()
        {
            var invalidExercise = new ExerciseRequestDTO
            {
                TopicId = "",
                IsTopicExam = null
            };

            var result = _validator.TestValidate(invalidExercise);

            result.ShouldHaveValidationErrorFor(e => e.TopicId);
            result.ShouldHaveValidationErrorFor(e => e.IsTopicExam);
        }

        [Test]
        public void ShouldNotHaveErrorWhenPropertiesAreSpecified()
        {
            var result = _validator.TestValidate(_validExercise);

            result.ShouldNotHaveValidationErrorFor(e => e.TopicId);
            result.ShouldNotHaveValidationErrorFor(e => e.IsTopicExam);
        }
    }
}