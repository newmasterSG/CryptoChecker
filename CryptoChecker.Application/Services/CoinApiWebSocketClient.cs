using CryptoChecker.Application.DTO;
using CryptoChecker.Application.Intefraces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace CryptoChecker.Application.Services
{
    public class CoinApiWebSocketClient : ICoinApiWebSocketClient
    {
        private readonly ClientWebSocket _webSocket;
        private readonly string _coinApiWebSocketUrl;
        private readonly string _apiKey;
        private readonly ILogger<CoinApiWebSocketClient> _logger;

        public CoinApiWebSocketClient(IOptions<CoinApiOptions> options, ILogger<CoinApiWebSocketClient> logger)
        {
            _webSocket = new();
            _coinApiWebSocketUrl = options.Value.URL;
            _apiKey = options.Value.ApiKey;
            _logger = logger;
        }

        public async Task GetInformationToken(PostSocketRequest assets, CancellationToken cancellationToken = default)
        {
            try
            {
                if (_webSocket.State != WebSocketState.Open)
                    await _webSocket.ConnectAsync(new Uri(_coinApiWebSocketUrl), cancellationToken);

                var subscribeMessage = new WebSocketMessage
                {
                    Type = "hello",
                    Heartbeat = false,
                    Apikey = _apiKey,
                    SubscribeDataType = ["trade"],
                    SubscribeFilterAssetId = assets.SubscribeFilterAssetId,
                };

                var messageJson = JsonSerializer.Serialize(subscribeMessage);
                var messageBytes = Encoding.UTF8.GetBytes(messageJson);

                await _webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, cancellationToken);

                var receiveBuffer = new byte[1024 * 4];
                var responseBuilder = new StringBuilder();

                while (_webSocket.State == WebSocketState.Open)
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), cancellationToken);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var response = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);

                        _logger.LogInformation(response);

                        responseBuilder.Append(response);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                if (_webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
                }
                _webSocket.Dispose();
            }
        }
    }
}
