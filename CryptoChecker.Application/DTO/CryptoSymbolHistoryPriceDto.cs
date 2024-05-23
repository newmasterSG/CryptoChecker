using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoChecker.Application.DTO
{
    public class CryptoSymbolHistoryPriceDto
    {
        public int Id { get; set; }
        public string? SymbolName { get; set; }
        public string? ExchangeName { get; set; }
        public string? SymbolType { get; set; }
        public string? TradeExchange { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }
        public DateTime DataQuoteStart { get; set; }
        public DateTime DataQuoteEnd { get; set; }
        public decimal Price { get; set; }
    }
}
