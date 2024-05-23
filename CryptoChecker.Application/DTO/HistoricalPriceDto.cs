namespace CryptoChecker.Application.DTO
{
    public class HistoricalPriceDto
    {
        public int Id { get; set; }
        public decimal AskPrice { get; set; }
        public decimal BidPrice { get; set; }
        public DateTime UpdateTime { get; set; }
        public CryptoSymbolHistoryPriceDto? CryptoSymbol { get; set; }
    }
}
