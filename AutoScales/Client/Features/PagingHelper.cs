using AutoScales.Shared.Models.PageModels;
using AutoScales.Shared.State;
using ChartJs.Blazor.Common.Axes.Ticks;

namespace AutoScales.Client.Features
{
    public class PagingHelper<T>
    {
        private List<T> _list;
        private int _spread;
        private int _number;
        private int _pageSize;
        private Func<Task> _getList;
        private Action _stateChanged;
        public PagingHelper(List<T> list, int spread, int pageSize, Func<Task> getList, Action stateChanged)
        {
            _list = list;
            _spread = spread;
            _pageSize = pageSize;
            _getList = getList;
            _stateChanged = stateChanged;
        }

        public List<T> List { get => _list; set => _list = value; }
        public int Number { get => _number; set => _number = value; }

        public PageParameters Parameters { get; set; } = new();
        public Metadata MetaDatas { get; set; } = new();
        public List<PagingLink> Links { get; set; } = new List<PagingLink>();

        public async Task SelectedPage(int page)
        {
            Parameters.PageNumber = page;
            await _getList.Invoke();
            _stateChanged.Invoke();
        }

        public async Task OnSelectedPage(PagingLink link)
        {
            if (link.Page == MetaDatas.CurrentPage || !link.Enabled)
                return;
            MetaDatas.CurrentPage = link.Page;
            await SelectedPage(link.Page);
            CreatePaginationLinks();
            _stateChanged.Invoke();
        }

        public void CreatePaginationLinks()
        {
            if (_list.Count > 0)
            {
                Links = new List<PagingLink>();
                Links.Add(new PagingLink(MetaDatas.CurrentPage - 1, MetaDatas.HasPrevious, "Prev"));
                for (int i = 1; i <= MetaDatas.TotalPages; i++)
                {
                    if (i >= MetaDatas.CurrentPage - _spread && i <= MetaDatas.CurrentPage + _spread)
                    {
                        Links.Add(new PagingLink(i, true, i.ToString()) { Active = MetaDatas.CurrentPage == i });
                    }
                }
                Links.Add(new PagingLink(MetaDatas.CurrentPage + 1, MetaDatas.HasNext, "Next"));
                _number = _pageSize;
                _number = _number * (MetaDatas.CurrentPage - 1);
            }
        }
    }
}
