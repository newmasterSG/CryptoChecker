using System.Text.Json.Serialization;

namespace CryptoChecker.Application.DTO
{
    public class WebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("apikey")]
        public string Apikey { get; set; }
        [JsonPropertyName("heartbeat")]
        public bool Heartbeat { get; set; }
        [JsonPropertyName("subscribe_data_type")]
        public string[] SubscribeDataType { get; set; }
        [JsonPropertyName("subscribe_filter_symbol_id")]
        public string[] SubscribeFilterAssetId { get; set; }
    }
}
