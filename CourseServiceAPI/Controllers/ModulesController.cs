using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Module.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CourseServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModulesController
    {
        private readonly ILogger<ModulesController> _logger;
        private readonly IModuleService _moduleService;

        public ModulesController(ILogger<ModulesController> logger, IModuleService moduleService)
        {
            _logger = logger;
            _moduleService = moduleService;
        }

        [HttpGet]
        public async Task<IEnumerable<ModuleResponseDTO>> GetModules()
        {
            var modules = await _moduleService.GetModulesAsync();
            return modules.Select(Mapper.MapToModuleResponseDto);
        }

        [HttpGet("")]

        [HttpPost]

        [HttpPut]

        [HttpDelete]
    }
}
