using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;

namespace Restaurant.Presentation.Controllers
{
    [ApiController]
    [Route("api/tables")]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService ?? throw new ArgumentNullException(nameof(tableService));
        }

        [HttpGet]
        public async Task<ActionResult<List<Table>>> GetAll()
        {
            var tables = await _tableService.GetAllTables();
            return Ok(tables);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Table>> GetById(Guid id)
        {
            var table = await _tableService.GetTableById(id);
            if (table == null)
                return NotFound();

            return Ok(table);
        }

        [HttpGet("hall/{hallId:guid}")]
        public async Task<ActionResult<List<Table>>> GetByHall(Guid hallId)
        {
            var tables = await _tableService.GetTablesByHallId(hallId);
            return Ok(tables);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddTableRequest request)
        {
            await _tableService.AddTable(request);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _tableService.DeleteTable(id);
            return Ok();
        }
    }
}
