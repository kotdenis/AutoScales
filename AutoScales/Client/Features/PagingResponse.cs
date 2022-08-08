using AutoScales.Shared.Models.PageModels;

namespace AutoScales.Client.Features
{
    public class PagingResponse<T> where T : class
    {
        public List<T> Items { get; set; } = new List<T>();
        public Metadata MetaData { get; set; } = new();
    }
}
