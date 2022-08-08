using AutoScales.Shared.Models;
using AutoScales.Shared.Models.PageModels;
using AutoScales.Shared.Views;

namespace AutoScales.Core.Services.Interfaces
{
    public interface IJournalService
    {
        Task<PagedList<JournalView>> GetJournalPageAsync(PageParameters parameters, CancellationToken ct);
        Task<PagedList<JournalView>> GetJournalPageBySearchAsync(SearchModel searchModel, PageParameters parameters, CancellationToken ct);
    }
}
