using CourseServiceAPI.Models.Exercise;

namespace CourseServiceAPI.Interfaces
{
    public interface IExerciseService
    {
        IEnumerable<Exercise> GetExercises();

        Exercise CreateExercise(Exercise exercise);

        void CompleteExercise(Guid exerciseId, List<List<AnsweredQuestion>> answeredQuestions);

        Exercise GetExerciseById(Guid id);

        Exercise PutExerciseById(Guid id, Exercise exercise);

        void DeleteExercise(Guid id);
    }
}