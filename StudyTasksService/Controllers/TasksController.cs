using Microsoft.AspNetCore.Mvc;
using StudyTasksService.Models;
using StudyTasksService.Services;

namespace StudyTasksService.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class TasksController : ControllerBase
    {
        private readonly TaskRepository _repository;

        public TasksController(TaskRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<List<StudyTask>> GetAll()
        {
            return _repository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<StudyTask> Get(int id)
        {
            var task = _repository.GetById(id);

            if (task == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorCode = "NOT_FOUND",
                    Message = "Task not found",
                    RequestId = HttpContext.Items["RequestId"]?.ToString()
                });
            }

            return task;
        }

        [HttpPost]
        public ActionResult<StudyTask> Create(CreateTaskRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "Title cannot be empty",
                    RequestId = HttpContext.Items["RequestId"]?.ToString()
                });
            }

            if (request.Difficulty < 0)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "Difficulty must be non-negative",
                    RequestId = HttpContext.Items["RequestId"]?.ToString()
                });
            }

            var task = _repository.Create(request);

            return Created($"/api/items/{task.Id}", task);
        }
    }
}