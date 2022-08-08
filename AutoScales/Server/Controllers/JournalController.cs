using AutoScales.Core.Services.Interfaces;
using AutoScales.Shared.Models;
using AutoScales.Shared.Models.PageModels;
using AutoScales.Shared.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoScales.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly IJournalService _journalService;
        public JournalController(IJournalService journalService)
        {
            _journalService = journalService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<JournalView>>> GetJournal([FromQuery] PageParameters parameters, CancellationToken ct = default)
        {
            parameters.PageSize = 12;
            var result = await _journalService.GetJournalPageAsync(parameters, ct);
            if (result == null)
                return NotFound();
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<PagedList<JournalView>>> GetJournalBySearch([FromBody]SearchModel searchModel, [FromQuery] PageParameters parameters, 
            CancellationToken ct = default)
        {
            parameters.PageSize = 12;
            var result = await _journalService.GetJournalPageBySearchAsync(searchModel, parameters, ct);
            if (result == null)
                return NotFound();
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));
            return Ok(result);
        }
    }
}
