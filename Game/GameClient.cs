using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Game2;

namespace Game2.Model
{
    public class Client
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        GameControllerClient _controllerClient;
        private bool _isRunning = true;



        public Client()
        {
            _tcpClient = new TcpClient();
            _controllerClient = new GameControllerClient();
        }

        public async Task StartAsync(string address = "127.0.0.1", int port = 5555)
        {
            try
            {
                await _tcpClient.ConnectAsync(address, port);
                _stream = _tcpClient.GetStream();

                Task listeningTask = ListenToServerAsync();
                Task inputTask = HandleKeyInputAsync();
                await Task.WhenAny(listeningTask, inputTask);
            }
            catch (Exception ex)
            {
                _controllerClient._view.renderer.Clear();
                _controllerClient._view.renderer.ShowMessage($"Connection error: {ex.Message}");
            }
            finally
            {
                _isRunning = false;
                _tcpClient?.Close();
                _controllerClient._view.renderer.ShowMessage($"Disconnected.");
            }
        }

        private async Task ListenToServerAsync()
        {
            try
            {
                while (_isRunning && _tcpClient.Connected)
                {
                    if (!_stream.DataAvailable) { await Task.Delay(50); continue; }

                    byte[] lengthBuffer = new byte[4];
                    int bytesRead = await _stream.ReadAsync(lengthBuffer, 0, 4);
                    if (bytesRead < 4) break;

                    int msgLength = BitConverter.ToInt32(lengthBuffer, 0);

                    if (msgLength <= 0) break;

                    byte[] msgBuffer = new byte[msgLength];
                    bytesRead = await _stream.ReadAsync(msgBuffer, 0, msgLength);
                    if (bytesRead < msgLength) break;

                    string jsonString = Encoding.UTF8.GetString(msgBuffer);
                    NetworkMessage? networkMsg = JsonSerializer.Deserialize<NetworkMessage>(jsonString);

                    if (networkMsg == null) continue;

                    switch (networkMsg.Type)
                    {
                        case MessageType.AssignIdAndBoard:
                            var payload = networkMsg.GetPayload<AssignIdAndBoardPayload>();
                            _controllerClient.InitializeGame(payload.InitialBoard.MazeBuffer, payload.ClientId, payload.instructions, payload.Strategy);
                            break;
                        case MessageType.BoardUpdate:
                            MazeDto maze = networkMsg.GetPayload<MazeDto>();
                            _controllerClient.UpdateMaze(maze.MazeBuffer);
                            break;
                        case MessageType.Error:
                            _controllerClient._view.renderer.ShowMessage($"SERVER ERROR: {networkMsg.GetPayload<string>()}");
                            break;
                        case MessageType.EndOfTheGame:
                            _isRunning = false;
                            _controllerClient._view.ShowMessage("You Lose!!!");
                            break;
                    }
                }
            }
            catch (Exception ex)
            { // ignore
                _controllerClient._view.renderer.Clear();
                _controllerClient._view.renderer.ShowMessage("No server");
            }
            finally {
                _isRunning = false;
                _tcpClient?.Close();
                _controllerClient._view.renderer.ShowMessage("Disconnected. No server.");
            }
        }

        private async Task HandleKeyInputAsync()
        {
            await Task.Run(async () =>
            {

                while (_isRunning)
                {
                    while (_controllerClient.gameState.Player_id == -1)
                    {
                        await Task.Delay(100);
                    }

                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(intercept: true).Key;
                        var actionPayload = _controllerClient.KeyToAction(key);

                        if (actionPayload != null)
                        {
                            var message = NetworkMessage.Create(MessageType.PlayerAction, actionPayload);
                            await SendMessageAsync(message);
                        }
                    }

                    //await Task.Delay(20);
                }
            });
        }

        private async Task SendMessageAsync(NetworkMessage message)
        {
            if (!_tcpClient.Connected || !_isRunning) return;
            try
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                byte[] lengthPrefix = BitConverter.GetBytes(jsonBytes.Length);
                await _stream.WriteAsync(lengthPrefix, 0, lengthPrefix.Length);
                await _stream.WriteAsync(jsonBytes, 0, jsonBytes.Length);
            }
            catch { _isRunning = false; /* Ignoruj */ }
        }
    }
}
