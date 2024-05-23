using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoChecker.Application.DTO
{
    public class CoinApiOptions() 
    {
        public const string Position = "CoinApiWebSocket";
        public string URL { get; set; }
        public string ApiKey { get; set; }
    };
}
