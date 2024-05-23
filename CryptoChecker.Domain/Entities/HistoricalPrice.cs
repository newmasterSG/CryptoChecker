namespace CryptoChecker.Domain.Entities
{
    public class HistoricalPrice
    {
        public int Id { get; set; }
        public decimal AskPrice { get; set; }
        public decimal BidPrice { get; set; }
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        public CryptoSymbol? CryptoSymbol { get; set; }
    }
}
