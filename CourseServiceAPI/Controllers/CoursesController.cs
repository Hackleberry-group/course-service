using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Course;
using CourseServiceAPI.Models.Course.DTOs;
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
        private readonly IModuleService _moduleService;

        public CoursesController(ILogger<CoursesController> logger, ICourseService courseService, IModuleService moduleService)
        {
            _logger = logger;
            _courseService = courseService;
            _moduleService = moduleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseResponseDTO>>> GetCourses()
        {
            var courses = await _courseService.GetCoursesAsync();

            if (courses == null)
            {
                return NotFound();
            }

            return Ok(courses.Select(Mapper.MapToCourseResponseDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseResponseDTO>> GetCourseById(Guid id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [HttpGet("{id}/modules")]
        public async Task<IEnumerable<ModuleResponseDTO>> GetModulesByCourseId(Guid courseId)
        {
            var modules = await _moduleService.GetModulesByCourseIdAsync(courseId);
            return modules.Select(Mapper.MapToModuleResponseDto);
        }


        [HttpPost]
        public async Task<ActionResult<CourseResponseDTO>> Post([FromBody] CourseRequestDTO courseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCourse = await _courseService.CreateCourseAsync(Mapper.MapToCourse(courseDTO));
            return Ok(Mapper.MapToCourseResponseDto(createdCourse));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CourseResponseDTO>> Put(Guid id, [FromBody] CourseRequestDTO courseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var updatedCourse = await _courseService.PutCourseByIdAsync(id, Mapper.MapToCourse(courseDTO));
            return Ok(updatedCourse);
        }
    }
}
