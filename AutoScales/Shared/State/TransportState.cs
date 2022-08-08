using AutoScales.Shared.Dtos;
using AutoScales.Shared.Models;
using AutoScales.Shared.Views;

namespace AutoScales.Shared.State
{
    public class TransportState
    {
        public List<ForDayModel> ForDayModels { get; set; } = new List<ForDayModel>();
        public List<TransportView> TransportViews { get; set; } = new List<TransportView>();
        public List<TransportDto> TransportDtos { get; set; } = new List<TransportDto>();
        public TransportDto TransportDto { get; set; } = new();
    }
}
