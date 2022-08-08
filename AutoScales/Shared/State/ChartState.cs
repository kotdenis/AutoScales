using ChartJs.Blazor;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;

namespace AutoScales.Shared.State
{
    public class ChartState
    {
        public ChartState()
        {
            TransportBarConfig = new BarConfig
            {
                Options = new BarOptions
                {
                    Responsive = true,
                    Legend = new Legend
                    {
                        Position = ChartJs.Blazor.Common.Enums.Position.Bottom
                    },
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "Quantity of axles and their loads"
                    }
                }
            };
        }
        public BarConfig TransportBarConfig { get; set; }

        public Chart TransportChart { get; set; }
    }
}
