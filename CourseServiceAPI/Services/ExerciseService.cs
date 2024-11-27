using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;

namespace CourseServiceAPI.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly ITableStorageQueryService _tableStorageQueryService;
        private readonly ITableStorageCommandService _tableStorageCommandService;
        private const string TableName = EntityConstants.ExerciseTableName;
        private const string PartitionKey = EntityConstants.ExercisePartitionKey;

        public ExerciseService(ITableStorageQueryService tableStorageQueryService, ITableStorageCommandService tableStorageCommandService)
        {
            _tableStorageQueryService = tableStorageQueryService;
            _tableStorageCommandService = tableStorageCommandService;
        }

        public async Task<IEnumerable<Exercise>> GetExercisesAsync()
        {
            return await _tableStorageQueryService.GetAllEntitiesAsync<Exercise>(TableName);
        }

        public async Task<Exercise> GetExerciseByIdAsync(Guid id)
        {
            return await _tableStorageQueryService.GetEntityAsync<Exercise>(TableName, PartitionKey, id.ToString());
        }

        public async Task<Exercise> PutExerciseByIdAsync(Guid id, Exercise exercise)
        {
            exercise.PartitionKey = PartitionKey;
            exercise.RowKey = id.ToString();
            await _tableStorageCommandService.UpdateEntityAsync(TableName, exercise);
            return exercise;
        }


        public async Task<Exercise> CreateExerciseAsync(Exercise exercise)
        {
            await _tableStorageCommandService.AddEntityAsync(TableName, exercise);
            return exercise;
        }

        public async Task<ExerciseCompletionResponse> CompleteExerciseAsync(Guid exerciseId, List<List<AnsweredQuestion>> answeredQuestions)
        {
            // Fetch the exercise from the table storage
            var exercise = await _tableStorageQueryService.GetEntityAsync<Exercise>(TableName, PartitionKey, exerciseId.ToString());

            // Create hard-coded questions and add to the exercise
            exercise.Questions = CreateHardCodedQuestions();

            var questionsTotal = exercise.Questions.Count;

            // Calculate the number of correct answers
            var questionsCorrect = answeredQuestions.SelectMany(questionList => questionList).Count(answeredQuestion => IsAnswerCorrect(answeredQuestion, exercise));

            // Determine if the exercise is passed
            var isPassed = questionsCorrect >= (questionsTotal * 0.7); // Assuming 70% is the passing criteria

            // Calculate the streak multiplier, XP earned, and current user XP
            var streakMultiplier = 1.0; // Placeholder for actual streak multiplier logic
            var xpEarned = questionsCorrect * 10; // Assuming 10 XP per correct answer
            var currentUserXP = 1000; // Placeholder for actual user XP retrieval logic

            // Create the response
            var response = new ExerciseCompletionResponse
            {
                ExerciseId = exerciseId,
                QuestionsTotal = questionsTotal,
                QuestionsCorrect = questionsCorrect,
                IsPassed = isPassed,
                StreakMultiplier = streakMultiplier,
                XPEarned = xpEarned,
                CurrentUserXP = currentUserXP
            };

            return response;
        }

        private static List<Question> CreateHardCodedQuestions()
        {
            var correctAnswerId1 = "00000000-0000-0000-0000-000000000001";
            var correctAnswerId2 = "00000000-0000-0000-0000-000000000002";

            var questionId1 = "00000000-0000-0000-0000-000000000001";
            var questionId2 = "00000000-0000-0000-0000-000000000002";

            // Create hard-coded questions
            var questions = new List<Question>
    {
        new()
        {
            QuestionId = Guid.Parse(questionId1),
            Text = "What is the capital of France?",
            Answers = new List<Answer>
            {
                new()
                {
                    AnswerId = Guid.Parse(correctAnswerId1),
                    Text = "Paris",
                },
                new()
                {
                    AnswerId = Guid.NewGuid(),
                    Text = "London",
                }
            },
            CorrectAnswerId = Guid.Parse(correctAnswerId1)
        },
        new()
        {
            QuestionId = Guid.Parse(questionId2),
            Text = "What is the capital of Germany?",
            Answers = new List<Answer>
            {
                new()
                {
                    AnswerId = Guid.NewGuid(),
                    Text = "Paris",
                },
                new()
                {
                    AnswerId = Guid.NewGuid(),
                    Text = "London",
                },
                new()
                {
                    AnswerId = Guid.Parse(correctAnswerId2),
                    Text = "Berlin",
                },
                new()
                {
                    AnswerId = Guid.NewGuid(),
                    Text = "Madrid",
                }
            },
            CorrectAnswerId = Guid.Parse(correctAnswerId2)
        }
    };

            return questions;
        }


        private static bool IsAnswerCorrect(AnsweredQuestion answeredQuestion, Exercise exercise)
        {
            var question = exercise.Questions.FirstOrDefault(q => q.QuestionId == answeredQuestion.QuestionId);
            if (question == null)
            {
                return false;
            }
            return question.CorrectAnswerId == answeredQuestion.AnswerId;
        }



        public async Task DeleteExerciseAsync(Guid id)
        {
            await _tableStorageQueryService.DeleteEntityAsync(TableName, PartitionKey, id.ToString());
        }
    }
}
