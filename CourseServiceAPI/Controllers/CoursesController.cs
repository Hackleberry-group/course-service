using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Course;
using CourseServiceAPI.Models.Module.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CourseServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly ICourseService _courseService;

        public CoursesController(ILogger<CoursesController> logger, ICourseService courseService)
        {
            _logger = logger;
            _courseService = courseService;
        }

        [HttpGet]
        public IEnumerable<Course> GetCourses()
        {
            return _courseService.GetCourses();
        }

        [HttpGet("{id}")]
        public ActionResult<Course> GetCourseById(Guid id)
        {
            var course = _courseService.GetCourseById(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [HttpGet("{id}/modules")]
        public Task<IEnumerable<ModuleResponseDTO>> GetModulesByCourseId(Guid courseId)
        {
            var modules = await _moduleService.GetModulesByCourseIdAsync(courseId);
            return modules.Select(Mapper.MapToModuleResponseDto);
        }


        [HttpPost]
        public IActionResult Post([FromBody] Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCourse = _courseService.CreateCourse(course);
            return Ok(createdCourse);
        }
    }
}
