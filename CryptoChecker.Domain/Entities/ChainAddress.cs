namespace CryptoChecker.Domain.Entities
{
    public class ChainAddress
    {
        public ChainAddress()
        {
            CryptoCurrencies = new List<CryptoCurrency>();
        }

        public int Id { get; set; }
        public string ChainName { get; set; }

        public string NetworkId { get; set; }

        public string Address { get; set; }

        public virtual ICollection<CryptoCurrency> CryptoCurrencies { get; set; }
    }
}
