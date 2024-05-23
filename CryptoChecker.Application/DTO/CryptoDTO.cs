namespace CryptoChecker.Application.DTO
{
    public class CryptoDTO
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string Name { get; set; }
        public decimal PriceUsd { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }

        public List<CryptoChainAddressDTO> CryptoChains { get; set; }
    }
}
