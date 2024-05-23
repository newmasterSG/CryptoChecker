using System.Text.Json.Serialization;

namespace CryptoChecker.Application.DTO.Responses
{
    public class CryptoSymbolResponse
    {
        [JsonPropertyName("symbol_id")]
        public string SymbolId { get; set; }

        [JsonPropertyName("exchange_id")]
        public string ExchangeId { get; set; }

        [JsonPropertyName("symbol_type")]
        public string SymbolType { get; set; }

        [JsonPropertyName("asset_id_base")]
        public string AssetIdBase { get; set; }

        [JsonPropertyName("asset_id_quote")]
        public string AssetIdQuote { get; set; }

        [JsonPropertyName("asset_id_unit")]
        public string AssetIdUnit { get; set; }

        [JsonPropertyName("future_contract_unit")]
        public double FutureContractUnit { get; set; }

        [JsonPropertyName("future_contract_unit_asset")]
        public string FutureContractUnitAsset { get; set; }

        [JsonPropertyName("data_start")]
        public DateTime DataStart { get; set; }

        [JsonPropertyName("data_end")]
        public DateTime DataEnd { get; set; }

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

        [JsonPropertyName("volume_1hrs")]
        public decimal Volume1Hrs { get; set; }

        [JsonPropertyName("volume_1hrs_usd")]
        public decimal Volume1HrsUsd { get; set; }

        [JsonPropertyName("volume_1day")]
        public decimal Volume1Day { get; set; }

        [JsonPropertyName("volume_1day_usd")]
        public decimal Volume1DayUsd { get; set; }

        [JsonPropertyName("volume_1mth")]
        public decimal Volume1Mth { get; set; }

        [JsonPropertyName("volume_1mth_usd")]
        public decimal Volume1MthUsd { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("symbol_id_exchange")]
        public string SymbolIdExchange { get; set; }

        [JsonPropertyName("asset_id_base_exchange")]
        public string AssetIdBaseExchange { get; set; }

        [JsonPropertyName("asset_id_quote_exchange")]
        public string AssetIdQuoteExchange { get; set; }

        [JsonPropertyName("price_precision")]
        public decimal PricePrecision { get; set; }

        [JsonPropertyName("size_precision")]
        public decimal SizePrecision { get; set; }
    }
}
