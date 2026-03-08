using Default.Application.DTOs.Requests;
using Default.Application.Services;
using Default.Models;
using Microsoft.AspNetCore.Mvc;

namespace Default.Controllers
{
    [ApiController]
    [Route("table")]
    public class TableController : ControllerBase
    {
        private readonly TableService _tableService;

        public TableController(TableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Table>>> GetAll()
        {
            var tables = await _tableService.GetAllTables();
            return Ok(tables);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Table>> GetById(Guid id)
        {
            var table = await _tableService.GetTableById(id);
            if (table == null)
                return NotFound();

            return Ok(table);
        }

        [HttpGet("hall/{hallId}")]
        public async Task<ActionResult<List<Table>>> GetByHall(Guid hallId)
        {
            var tables = await _tableService.GetTablesByHallId(hallId);
            return Ok(tables);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddTableRequest request)
        {
            try
            {
                await _tableService.AddTable(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _tableService.DeleteTable(id);
            return Ok();
        }
    }
}
