namespace CryptoChecker.Application.DTO
{
    public class GetCrypto
    {
        public List<CryptoDTO> Cryptos { get; set; }
        public int PageCount { get; set; }
    }
}
