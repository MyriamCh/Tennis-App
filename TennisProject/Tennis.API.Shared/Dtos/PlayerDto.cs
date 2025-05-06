namespace Tennis.API.Shared.Dtos
{
    public class PlayerDto
    {
        public string? FullName { get; set; }
        public string? ShortName { get; set; }
        public string? Picture { get; set; }
        public string? Sex { get; set; }
        public int Age { get; set; }
        public string? CountryCode { get; set; }
        public int Rank { get; set; }
    }
}
