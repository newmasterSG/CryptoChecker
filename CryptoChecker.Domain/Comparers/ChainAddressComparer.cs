using CryptoChecker.Domain.Entities;

namespace CryptoChecker.Domain.Comparers
{
    public class ChainAddressComparer : IEqualityComparer<ChainAddress>
    {
        public bool Equals(ChainAddress x, ChainAddress y)
        {
            return x.Address == y.Address &&
                   x.ChainName == y.ChainName;
        }

        public int GetHashCode(ChainAddress obj)
        {
            return HashCode.Combine(obj.Address, obj.ChainName);
        }
    }
}
