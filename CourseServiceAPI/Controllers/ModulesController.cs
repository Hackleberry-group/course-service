using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Module.DTOs;
using CourseServiceAPI.Models.Topic.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CourseServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModulesController
    {
        private readonly ILogger<ModulesController> _logger;
        private readonly IModuleService _moduleService;
        private readonly ITopicService _topicService;

        public ModulesController(ILogger<ModulesController> logger, IModuleService moduleService, ITopicService topicService)
        {
            _logger = logger;
            _moduleService = moduleService;
            _topicService = topicService;
        }

        [HttpGet]
        public async Task<IEnumerable<ModuleResponseDTO>> GetModules()
        {
            var modules = await _moduleService.GetModulesAsync();
            return modules.Select(Mapper.MapToModuleResponseDto);
        }

        [HttpGet("{id}")]
        public async Task<ModuleResponseDTO> GetModuleById(Guid id)
        {
            var module = await _moduleService.GetModuleByIdAsync(id);
            return Mapper.MapToModuleResponseDto(module);
        }

        [HttpGet("{id}/topics")]
        public async Task<IEnumerable<TopicResponseDto>> GetTopicsByModuleId(Guid moduleId)
        {
            var topics = await _topicService.GetTopicsByModuleIdAsync(moduleId);
            return topics.Select(Mapper.MapToTopicResponseDto);
        }

        [HttpPost]
        public async Task<ModuleResponseDTO> Post([FromBody] ModuleRequestDTO moduleDTO)
        {
            var module = Mapper.MapToModule(moduleDTO);
            var createdModule = await _moduleService.CreateModuleAsync(module);

            return Mapper.MapToModuleResponseDto(createdModule);
        }

        [HttpPut]
        public async Task<ModuleResponseDTO> Put([FromRoute] Guid moduleId, [FromBody] ModuleRequestDTO moduleDTO)
        {
            var module = Mapper.MapToModule(moduleDTO);
            var updatedModule = await _moduleService.PutModuleByIdAsync(moduleId, module);

            return Mapper.MapToModuleResponseDto(updatedModule);
        }

        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _moduleService.DeleteModuleAsync(id);
        }
    }
}
