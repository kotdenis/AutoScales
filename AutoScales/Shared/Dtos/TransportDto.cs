using System.ComponentModel.DataAnnotations;

namespace AutoScales.Shared.Dtos
{
    public class TransportDto : BaseDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Number { get; set; } = string.Empty;
        [Required]
        [Range(minimum:1000, maximum:100000, ErrorMessage = "Weight must be in range 1000 - 100000")]
        public int Weight { get; set; }
        [Required]
        [Range(minimum: 2, maximum: 8, ErrorMessage = "Number of axles must be in range 2 - 8")]
        public int AxisNumber { get; set; }
        [Required]
        public string Cargo { get; set; } = string.Empty;
    }
}
