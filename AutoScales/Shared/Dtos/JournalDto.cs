namespace AutoScales.Shared.Dtos
{
    public class JournalDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public int Weight { get; set; }
        public string Cargo { get; set; } = string.Empty;
        public DateTime WeighinDate { get; set; }
        public string Time { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
    }
}
