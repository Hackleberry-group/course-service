using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Exercise;

namespace CourseServiceAPI.Services;

public class ExerciseService : IExerciseService
{
    public IEnumerable<Exercise> GetExercises()
    {
        // Get exercises from the database but for now return a dummy list
        return new List<Exercise>
        {
            new() { Id = Guid.NewGuid(), IsTopicExam = false, TopicId = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), IsTopicExam = true, TopicId = Guid.NewGuid() }
        };
    }

    // Get Exercise by Id
    public Exercise GetExerciseById(Guid id)
    {
        return new Exercise { Id = id, IsTopicExam = false, TopicId = Guid.NewGuid() };
    }

    // Put exercise by Id
    public Exercise PutExerciseById(Guid id, Exercise exercise)
    {
        var updatedExercise = new Exercise
        {
            Id = id,
            IsTopicExam = exercise.IsTopicExam, TopicId = exercise.TopicId
        };

        return updatedExercise;
    }

    public Exercise CreateExercise(Exercise exercise)
    {
        // For now return a dummy exercise
        var createdExercise = new Exercise
        {
            Id = Guid.NewGuid(),
            IsTopicExam = exercise.IsTopicExam,
            TopicId = exercise.TopicId
        };

        return createdExercise;
    }

    public void CompleteExercise(Guid exerciseId, List<List<AnsweredQuestion>> answeredQuestions)
    {
        // For now add a dummy completion with the exerciseId and answeredQuestions without any database logic

        var completion = new ExerciseCompletion { ExerciseId = exerciseId, AnsweredQuestions = answeredQuestions };

        Console.WriteLine($"Exercise completion: {completion.ExerciseId}");
        foreach (var question in completion.AnsweredQuestions)
        {
            Console.WriteLine("Question:");
            foreach (var answeredQuestion in question)
            {
                Console.WriteLine($"Answered question: {answeredQuestion.QuestionId}");
                Console.WriteLine($"Answer: {answeredQuestion.AnswerId}");
            }
        }
    }

    public void DeleteExercise(Guid id)
    {
        // Delete exercise from the database
        Console.WriteLine($"Exercise with id {id} deleted");
    }
}