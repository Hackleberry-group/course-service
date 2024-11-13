using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ILogger<CourseController> _logger;
        private readonly ICourseService _courseService;

        public CourseController(ILogger<CourseController> logger, ICourseService courseService)
        {
            _logger = logger;
            _courseService = courseService;
        }

        [HttpGet]
        public IEnumerable<Course> Get()
        {
            return _courseService.GetCourses();
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
