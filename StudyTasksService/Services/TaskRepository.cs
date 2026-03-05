using StudyTasksService.Models;

namespace StudyTasksService.Services
{
    public class TaskRepository
    {
        private readonly List<StudyTask> _tasks = new();
        private int _nextId = 1;

        private readonly object _lock = new();

        public List<StudyTask> GetAll()
        {
            return _tasks;
        }

        public StudyTask GetById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public StudyTask Create(CreateTaskRequest request)
        {
            lock (_lock)
            {
                var task = new StudyTask
                {
                    Id = _nextId++,
                    Title = request.Title,
                    Difficulty = request.Difficulty
                };

                _tasks.Add(task);
                return task;
            }
        }
    }
}