using CryptoChecker.Application.DTO;
using CryptoChecker.Application.DTO.Responses;
using CryptoChecker.Domain.Entities;

namespace CryptoChecker.Application.Mapping
{
    public static class Mappers
    {
        public static CryptoDTO CryptoToDTO(CryptoCurrency crypto) => new CryptoDTO
        {
            Id = crypto.Id,
            AssetName = crypto.AssetName,
            Name = crypto.Name,
            PriceUsd = crypto.PriceUsd,
            DataStart = crypto.DataStart,
            DataEnd = crypto.DataEnd,
            CryptoChains = crypto.ChainAddresses.Select(chainAddress => new CryptoChainAddressDTO
            {
                Id = chainAddress.Id,
                ChainName = chainAddress.ChainName,
                NetworkId = chainAddress.NetworkId,
                Address = chainAddress.Address
            }).ToList()
        };

        public static CryptoSymbol DTOToSymbol(CryptoSymbolResponse response) => new CryptoSymbol
        {
            SymbolName = response.SymbolId,
            ExchangeName = response.ExchangeId,
            SymbolType = response.SymbolType,
            TradeExchange = response.AssetIdQuoteExchange,
            DataStart = response.DataStart,
            DataEnd = response.DataEnd,
            DataQuoteStart = response.DataQuoteStart,
            DataQuoteEnd = response.DataQuoteEnd,
            Price = response.Price,
        };


        public static HistoricalPrice ResponseToPrice(CryptoQuote cryptoQuote) => new HistoricalPrice
        {
            AskPrice = cryptoQuote.AskPrice,
            BidPrice = cryptoQuote.BidPrice,
            UpdateTime = cryptoQuote.TimeExchange,
        };
        public static HistoricalPriceDto ToDto(this HistoricalPrice historicalPrice)
        {
            return new HistoricalPriceDto
            {
                Id = historicalPrice.Id,
                AskPrice = historicalPrice.AskPrice,
                BidPrice = historicalPrice.BidPrice,
                UpdateTime = historicalPrice.UpdateTime,
                CryptoSymbol = historicalPrice.CryptoSymbol?.ToDto()
            };
        }

        public static HistoricalPrice ToModel(this HistoricalPriceDto historicalPriceDto)
        {
            return new HistoricalPrice
            {
                Id = historicalPriceDto.Id,
                AskPrice = historicalPriceDto.AskPrice,
                BidPrice = historicalPriceDto.BidPrice,
                UpdateTime = historicalPriceDto.UpdateTime,
                CryptoSymbol = historicalPriceDto.CryptoSymbol?.ToModel()
            };
        }
        public static CryptoSymbolHistoryPriceDto ToDto(this CryptoSymbol cryptoSymbol)
        {
            return new CryptoSymbolHistoryPriceDto
            {
                Id = cryptoSymbol.Id,
                SymbolName = cryptoSymbol.SymbolName,
                ExchangeName = cryptoSymbol.ExchangeName,
                SymbolType = cryptoSymbol.SymbolType,
                TradeExchange = cryptoSymbol.TradeExchange,
                DataStart = cryptoSymbol.DataStart,
                DataEnd = cryptoSymbol.DataEnd,
                DataQuoteStart = cryptoSymbol.DataQuoteStart,
                DataQuoteEnd = cryptoSymbol.DataQuoteEnd,
                Price = cryptoSymbol.Price
            };
        }

        public static CryptoSymbol ToModel(this CryptoSymbolHistoryPriceDto cryptoSymbolDto)
        {
            return new CryptoSymbol
            {
                Id = cryptoSymbolDto.Id,
                SymbolName = cryptoSymbolDto.SymbolName,
                ExchangeName = cryptoSymbolDto.ExchangeName,
                SymbolType = cryptoSymbolDto.SymbolType,
                TradeExchange = cryptoSymbolDto.TradeExchange,
                DataStart = cryptoSymbolDto.DataStart,
                DataEnd = cryptoSymbolDto.DataEnd,
                DataQuoteStart = cryptoSymbolDto.DataQuoteStart,
                DataQuoteEnd = cryptoSymbolDto.DataQuoteEnd,
                Price = cryptoSymbolDto.Price
            };
        }
    }
}
