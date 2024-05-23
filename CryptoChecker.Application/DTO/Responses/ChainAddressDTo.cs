using System.Text.Json.Serialization;

namespace CryptoChecker.Application.DTO.Responses
{
    public class ChainAddressDTo
    {
        [JsonPropertyName("chain_id")]
        public string ChainId { get; set; }

        [JsonPropertyName("network_id")]
        public string NetworkId { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }
}
