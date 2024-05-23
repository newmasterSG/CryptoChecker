using System.Text.Json.Serialization;

namespace CryptoChecker.Application.DTO.Responses
{
    public class CurrencyResponse
    {
        [JsonPropertyName("asset_id")]
        public string AssetId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type_is_crypto")]
        public int TypeIsCrypto { get; set; }

        [JsonPropertyName("data_quote_start")]
        public DateTime DataQuoteStart { get; set; }

        [JsonPropertyName("data_quote_end")]
        public DateTime DataQuoteEnd { get; set; }

        [JsonPropertyName("data_orderbook_start")]
        public DateTime DataOrderbookStart { get; set; }

        [JsonPropertyName("data_orderbook_end")]
        public DateTime DataOrderbookEnd { get; set; }

        [JsonPropertyName("data_trade_start")]
        public DateTime DataTradeStart { get; set; }

        [JsonPropertyName("data_trade_end")]
        public DateTime DataTradeEnd { get; set; }

        [JsonPropertyName("data_symbols_count")]
        public long DataSymbolsCount { get; set; }

        [JsonPropertyName("volume_1hrs_usd")]
        public double Volume1hrsUsd { get; set; }

        [JsonPropertyName("volume_1day_usd")]
        public double Volume1dayUsd { get; set; }

        [JsonPropertyName("volume_1mth_usd")]
        public double Volume1mthUsd { get; set; }

        [JsonPropertyName("price_usd")]
        public double PriceUsd { get; set; }

        [JsonPropertyName("id_icon")]
        public string IdIcon { get; set; }

        [JsonPropertyName("chain_addresses")]
        public List<ChainAddressDTo>? ChainAddresses { get; set; }

        [JsonPropertyName("data_start")]
        public DateTime DataStart { get; set; }

        [JsonPropertyName("data_end")]
        public DateTime DataEnd { get; set; }
    }
}
