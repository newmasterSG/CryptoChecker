using System.Text.Json.Serialization;

namespace CryptoChecker.Application.DTO.Responses
{
    public class CryptoQuote
    {
        [JsonPropertyName("symbol_id")]
        public string SymbolId { get; set; }

        [JsonPropertyName("time_exchange")]
        public DateTime TimeExchange { get; set; }

        [JsonPropertyName("time_coinapi")]
        public DateTime TimeCoinapi { get; set; }

        [JsonPropertyName("ask_price")]
        public decimal AskPrice { get; set; }

        [JsonPropertyName("ask_size")]
        public decimal AskSize { get; set; }

        [JsonPropertyName("bid_price")]
        public decimal BidPrice { get; set; }

        [JsonPropertyName("bid_size")]
        public decimal BidSize { get; set; }
    }
}
