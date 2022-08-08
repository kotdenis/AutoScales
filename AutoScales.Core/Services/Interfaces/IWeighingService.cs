using AutoScales.Shared.Models.PageModels;
using AutoScales.Shared.Models;
using AutoScales.Shared.Views;
using AutoScales.Shared.Dtos;

namespace AutoScales.Core.Services.Interfaces
{
    public interface IWeighingService
    {
        Task<TransportView> GetRandomTransportAsync(CancellationToken ct);
        Task<bool> SaveTransportWeightAsync(JournalView journalView, CancellationToken ct);
        Task<PagedList<ForDayModel>> GetTransportForDayAsync(PageParameters parameters, CancellationToken ct);
        Task<PagedList<TransportDto>> GetTransportForAdminAsync(PageParameters parameters, CancellationToken ct);
        Task<bool> CreateTransportAsync(TransportDto transportDto, CancellationToken ct);
        Task<bool> UpdateTransportAsync(TransportDto transportDto, CancellationToken ct);
        Task<bool> DeleteSoftAsync(TransportDto transportDto, CancellationToken ct);
    }
}
