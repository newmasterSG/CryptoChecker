using System.Text.Json.Serialization;

namespace CryptoChecker.Domain.Entities
{
    public class CryptoSymbol
    {
        public CryptoSymbol()
        {
            HistoricalPrices = new List<HistoricalPrice>();
        }

        public int Id { get; set; }
        public string? SymbolName { get; set; }
        public string? ExchangeName { get; set; }
        public string? SymbolType { get; set; }
        public string? TradeExchange { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }
        public DateTime DataQuoteStart { get; set; }
        public DateTime DataQuoteEnd { get; set; }
        public decimal Price { get; set; }
        public int? CryptoId { get; set; }

        public virtual CryptoCurrency? Crypto { get; set; }

        public virtual ICollection<HistoricalPrice> HistoricalPrices { get; set; }
    }
}
