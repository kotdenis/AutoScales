using AutoScales.Core.Services.Interfaces;
using AutoScales.Shared.Constants;
using AutoScales.Shared.Dtos;
using AutoScales.Shared.Models;
using AutoScales.Shared.Models.PageModels;
using AutoScales.Shared.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoScales.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WeighingController : ControllerBase
    {
        private readonly IWeighingService _weighingService;
        public WeighingController(IWeighingService weighingService)
        {
            _weighingService = weighingService;
        }

        [HttpGet("random")]
        public async Task<ActionResult<JournalView>> GetRandomTransport(CancellationToken ct = default)
        {
            var transport = await _weighingService.GetRandomTransportAsync(ct);
            if(transport == null)
                return NotFound();
            return Ok(transport);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveWeighing([FromBody] JournalView journalView, CancellationToken ct = default)
        {
            var result = await _weighingService.SaveTransportWeightAsync(journalView, ct);
            if(result)
                return Ok();
            return BadRequest();
        }

        [HttpGet("forday")]
        public async Task<ActionResult<PagedList<ForDayModel>>> GetTransportForDay([FromQuery]PageParameters parameters, CancellationToken ct = default)
        {
            parameters.PageSize = 12;
            var models = await _weighingService.GetTransportForDayAsync(parameters, ct);
            if (models == null)
                return BadRequest();
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(models.MetaData));
            return Ok(models);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpGet("admin-journal")]
        public async Task<ActionResult<PagedList<TransportDto>>> GetTransportsForAdmin([FromQuery] PageParameters parameters, CancellationToken ct = default)
        {
            parameters.PageSize = 5;
            var dtos = await _weighingService.GetTransportForAdminAsync(parameters, ct);
            if (dtos == null)
                return BadRequest();
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(dtos.MetaData));
            return Ok(dtos);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("create")]
        public async Task<ActionResult<bool>> CreateTransport(TransportDto transport, CancellationToken ct = default)
        {
            var result = await _weighingService.CreateTransportAsync(transport, ct);
            if(!result)
                return BadRequest();
            return Ok(result);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTransport([FromBody] TransportDto transportDto, CancellationToken ct = default)
        {
            var result = await _weighingService.UpdateTransportAsync(transportDto, ct);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSoft([FromBody] TransportDto transportDto, CancellationToken ct = default)
        {
            var result = await _weighingService.DeleteSoftAsync(transportDto, ct);
            if (!result)
                return BadRequest();
            return Ok(result);
        }
    }
}
