using AutoScales.Client.Features;
using AutoScales.Shared.Models.PageModels;
using AutoScales.Shared.Models;
using AutoScales.Shared.Views;
using AutoScales.Shared.Dtos;

namespace AutoScales.Client.Services.Interfaces
{
    public interface IWeighingService
    {
        Task<TransportView> GetRandomTransportAsync();
        Task<bool> SaveWeighingAsync(JournalView journalView);
        Task<PagingResponse<ForDayModel>> GetPagedForDayAsync(PageParameters parameters);
        Task<PagingResponse<TransportDto>> GetTransportsForAdminAsync(PageParameters parameters);
        Task<bool> CreateTransportAsync(TransportDto transportDto);
        Task<bool> UpdateTransportAsync(TransportDto transportDto);
        Task<bool> DeleteSoftAsync(TransportDto transportDto);
    }
}
