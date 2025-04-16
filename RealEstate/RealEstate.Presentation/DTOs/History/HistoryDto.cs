namespace RealEstate.Presentation.DTOs.History
{
    public record HistoryDto : BaseDto
    {
        public DateTime CompletedAt { get; set; }
        public string EstateAction { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
