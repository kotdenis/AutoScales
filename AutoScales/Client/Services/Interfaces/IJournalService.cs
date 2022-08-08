using AutoScales.Client.Features;
using AutoScales.Shared.Models;
using AutoScales.Shared.Models.PageModels;
using AutoScales.Shared.Views;

namespace AutoScales.Client.Services.Interfaces
{
    public interface IJournalService
    {
        Task<PagingResponse<JournalView>> GetPagedJournalAsync(PageParameters parameters);
        Task<PagingResponse<JournalView>> GetPagedJournalBySearchAsync(SearchModel searchModel, PageParameters parameters);
    }
}
