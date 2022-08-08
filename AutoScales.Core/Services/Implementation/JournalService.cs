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
    public class JournalService : IJournalService
    {
        private readonly IJournalRepository _journalRepository;
        private readonly IMapper _mapper;
        public JournalService(IJournalRepository journalRepository,
            IMapper mapper)
        {
            _journalRepository = journalRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<JournalView>> GetJournalPageAsync(PageParameters parameters, CancellationToken ct)
        {
            var journals = await _journalRepository.FindAllAsync(x => x.IsDeleted == false, ct);
            if (journals == null)
                throw new NullReferenceException(nameof(journals));
            var dtos = _mapper.ProjectTo<JournalDto>(journals.OrderByDescending(x => x.WeighinDate).AsQueryable());
            var views = _mapper.ProjectTo<JournalView>(dtos);
            var list = views.ToList();
            return PagedList<JournalView>.ToPagedList(views, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<JournalView>> GetJournalPageBySearchAsync(SearchModel searchModel, PageParameters parameters, CancellationToken ct)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            Func<Journal, bool> predicate = (j) => true;
            var date = searchModel.WeghingDate.Date;
            if (!string.IsNullOrEmpty(searchModel.CarPlate))
                predicate = (x) => x.Number == searchModel.CarPlate && x.WeighinDate.Date == date && x.IsDeleted == false;
            else
                predicate = (x) => x.WeighinDate.Date == date && x.IsDeleted == false;
            var journals = await _journalRepository.FindAllAsync(predicate, ct);
            var dtos = _mapper.ProjectTo<JournalDto>(journals.AsQueryable());
            var views = _mapper.ProjectTo<JournalView>(dtos);
            var list = views.OrderByDescending(x => x.WeighinDate).ToList();
            return PagedList<JournalView>.ToPagedList(views, parameters.PageNumber, parameters.PageSize);
        }
    }
}
