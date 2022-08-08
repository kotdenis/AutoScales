using AutoMapper;
using AutoScales.Core.Services.Interfaces;
using AutoScales.Data.Entities;
using AutoScales.Data.Repositories.Interfaces;
using AutoScales.Shared.Dtos;
using AutoScales.Shared.Models;
using AutoScales.Shared.Models.PageModels;
using AutoScales.Shared.Views;

namespace AutoScales.Core.Services.Implementation
{
    public class WeighingService : IWeighingService
    {
        private readonly IWeighingRepository _weighingRepository;
        private readonly ITransportQuantityRepository _quantityRepository;
        private readonly IJournalRepository _journalRepository;
        private readonly IMapper _mapper;
        public WeighingService(IWeighingRepository weighingRepository,
            ITransportQuantityRepository quantityRepository,
            IJournalRepository journalRepository,
            IMapper mapper)
        {
            _weighingRepository = weighingRepository;
            _quantityRepository = quantityRepository;
            _journalRepository = journalRepository;
            _mapper = mapper;
        }

        public async Task<TransportView> GetRandomTransportAsync(CancellationToken ct)
        {
            var transports = await _weighingRepository.GetAllAsync(ct);
            var dtos = _mapper.ProjectTo<TransportDto>(transports);
            var quantities = await _quantityRepository.GetAllAsync(ct);
            var quantity = quantities.Select(x => x.Quantity).FirstOrDefault();
            var random = new Random();
            var transportId = random.Next(quantity);
            var dto = dtos.Where(x => x.Id == transportId).FirstOrDefault();
            return _mapper.Map<TransportView>(dto);
        }

        public async Task<bool> SaveTransportWeightAsync(JournalView journalView, CancellationToken ct)
        {
            if (journalView == null)
                throw new ArgumentNullException(nameof(journalView));
            var dto = _mapper.Map<JournalDto>(journalView);
            var journal = _mapper.Map<Journal>(dto);
            journal.WeighinDate = DateTime.Now;
            journal.Date = DateTime.Now.ToShortDateString();
            journal.Time = DateTime.Now.ToShortTimeString();
            await _journalRepository.CreateAsync(journal, ct);
            return true;
        }

        public async Task<PagedList<ForDayModel>> GetTransportForDayAsync(PageParameters parameters, CancellationToken ct)
        {
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var journals = await _journalRepository.FindAllAsync(x => x.WeighinDate >= now && x.IsDeleted == false, ct);
            var models = new List<ForDayModel>();
            foreach (var journal in journals.OrderByDescending(x => x.Id))
                models.Add(new ForDayModel
                {
                    CarPlate = journal.Number,
                    Time = journal.Time,
                    Weight = journal.Weight
                });
            return PagedList<ForDayModel>.ToPagedList(models, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<TransportDto>> GetTransportForAdminAsync(PageParameters parameters, CancellationToken ct)
        {
            var transports = await _weighingRepository.FindAllAsync(x => x.IsDeleted == false, ct);
            if(transports == null)
                throw new NullReferenceException(nameof(transports));
            var dtos = _mapper.ProjectTo<TransportDto>(transports.OrderByDescending(x => x.Id).AsQueryable());
            var list = dtos.ToList();
            return PagedList<TransportDto>.ToPagedList(list, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<bool> CreateTransportAsync(TransportDto transportDto, CancellationToken ct)
        {
            if(transportDto == null)
                throw new ArgumentNullException(nameof(transportDto));
            var transport = _mapper.Map<Transport>(transportDto);
            await _weighingRepository.CreateAsync(transport, ct);
            return true;
        }

        public async Task<bool> UpdateTransportAsync(TransportDto transportDto, CancellationToken ct)
        {
            if(transportDto == null)
                throw new ArgumentNullException(nameof(transportDto));
            var transport = _mapper.Map<Transport>(transportDto);
            var updated = await _weighingRepository.UpdateAsync(transport, ct);
            return updated;
        }

        public async Task<bool> DeleteSoftAsync(TransportDto transportDto, CancellationToken ct)
        {
            if (transportDto == null)
                throw new ArgumentNullException(nameof(transportDto));
            var transport = _mapper.Map<Transport>(transportDto);
            var deleted = await _weighingRepository.SoftDeleteAsync(transport, ct);
            return deleted;
        }
    }
}
