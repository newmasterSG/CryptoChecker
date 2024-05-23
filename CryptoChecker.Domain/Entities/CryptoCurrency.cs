namespace CryptoChecker.Domain.Entities
{
    public class CryptoCurrency
    {
        public CryptoCurrency()
        {
            ChainAddresses = new List<ChainAddress>();

            CryptoSymbols = new List<CryptoSymbol>();
        }

        public int Id { get; set; }
        public string AssetName { get; set; }
        public string Name { get; set; }
        public decimal PriceUsd { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }

        public virtual ICollection<ChainAddress> ChainAddresses { get; set; }

        public virtual ICollection<CryptoSymbol> CryptoSymbols { get; set; }
    }
}
