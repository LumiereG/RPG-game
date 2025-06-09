using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net.Http;
using static System.Collections.Specialized.BitVector32;

namespace Game2.Model
{

    public class GameServer
    {
        private TcpListener _listener;
        private GameControllerServer _controller;

        private Dictionary<TcpClient, int> _clientIds = new();
        private List<TcpClient> _clients = new();
        private int _nextId = 1;
        private object _lock = new();
        private int _port;
        int MaxClient = 9;

        private readonly Dictionary<int, bool> _playerHasActed = new();
        private readonly ConcurrentQueue<PlayerAction> _actionQueue = new ConcurrentQueue<PlayerAction>();
        private Task? _gameLoopTask;
        public GameServer(int port = 5555)
        {
            _port = port;
            _controller = new GameControllerServer(4);
        }
        public async Task StartAsync()
        {
            _listener = new TcpListener(IPAddress.Loopback, _port);
            _listener.Start();
            _controller.AddLog($"[Server] Server started on port {_listener.LocalEndpoint.ToString()}. Waiting for connections...");

            _gameLoopTask = Task.Run(GameLoopAsync);

            try
            {
                while (true) 
                {
                    TcpClient client;
                    try
                    {
                        client = await _listener.AcceptTcpClientAsync(); 
                    }
                    catch (OperationCanceledException)
                    {
                        _controller.errors.Add("[Server] Server listener stopped by cancellation.");
                        break; 
                    }

                    int clientId;

                    if (_clients.Count < MaxClient)
                    {
                        lock (_lock)
                        {
                            clientId = _nextId++;
                            _clientIds[client] = clientId;
                            _clients.Add(client);
                            _playerHasActed[clientId] = false;
                            _controller.gameState.AddPlayer(clientId);
                        }
                        _controller.AddLog($"Client {clientId} connected.");
                        var dto = _controller.gameState.Maze.ToDto(clientId, _controller.instruction);
                        var payload = new AssignIdAndBoardPayload { ClientId = clientId, InitialBoard = dto, instructions = _controller.instruction, Strategy = _controller.strategy };
                        _ = SendMessageAsync(client, NetworkMessage.Create(MessageType.AssignIdAndBoard, payload));
                        BroadcastBoardState();

                        _ = Task.Run(() => ListenToClientAsync(client, clientId));
                    }
                    else
                    {
                        _controller.AddLog("[Server] Max clients reached. Rejecting new connection.");
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _controller.errors.Add($"Server listener error: {ex.Message}");
            }
            finally
            {
                _listener.Stop();
                _controller.errors.Add("[Server] Listener stopped.");
            }
        }

        public void EnqueueAction(PlayerAction action)
        {
            _actionQueue.Enqueue(action);
        }

        private async Task GameLoopAsync()
        {
            StrategyManager manager = new StrategyManager();

            while (true)
            {
                bool stateChanged = false;
                while (_actionQueue.TryDequeue(out PlayerAction? action))
                {
                    if (action != null && _clientIds.ContainsValue(action.PlayerId) && !_playerHasActed[action.PlayerId])
                    {
                        if (_controller.HandlePlayerAction(action.PlayerId, action.Type, action.Payload))
                        {
                            stateChanged = true;
                            lock (_lock)
                            {
                                _playerHasActed[action.PlayerId] = true;
                            }
                            if (_controller.gameState.Maze.GetPlayer(action.PlayerId).isDead)
                            {
                                DisconnectPlayer(action.PlayerId);
                            }
                        }
                    }
                }

                if (stateChanged)
                {
                    BroadcastBoardState();
                }

                lock (_lock)
                {
                    if (_playerHasActed.Count > 0 && _playerHasActed.Values.All(v => v))
                    {
                        Maze maze = _controller.gameState.Maze;
                        if (maze != null)
                        {
                            var enemiesToUpdate = maze.Enemies.Where(e => !e.IsDead).ToList(); // Get active enemies

                            manager.UpdateStrategies(maze.Enemies, maze);
                            foreach (Enemy enemy in enemiesToUpdate)
                            {
                                enemy.UpdateBehavior(maze);
                            }

                            var currentClientIds = _clientIds.Values.ToList(); 
                            foreach (var playerId in currentClientIds)
                            {
                                Player? p = maze.GetPlayer(playerId); 
                                if (p != null && p.isDead)
                                {
                                    if (_clientIds.Any(kvp => kvp.Value == playerId))
                                    {
                                        var client = _clientIds.First(kvp => kvp.Value == playerId).Key;
                                        var dto = maze.ToDto(playerId, _controller.instruction);
                                        var msg = NetworkMessage.Create(MessageType.EndOfTheGame, dto);
                                        _ = SendMessageAsync(client, msg);
                                    }
                                    DisconnectPlayer(playerId);
                                }
                            }
                        }

                        _controller.gameState.Notify();
                        BroadcastBoardState();
                        ResetTurnFlags();
                    }
                }

               await Task.Delay(100);


            }
        }

        private async Task ListenToClientAsync(TcpClient tcpClient, int clientId)
        {
            var stream = tcpClient.GetStream();
            try
            {
                while (true)
                {
                    byte[] lengthBuffer = new byte[4];
                    if (await stream.ReadAsync(lengthBuffer, 0, 4) < 4) break;
                    int msgLength = BitConverter.ToInt32(lengthBuffer, 0);
                    if (msgLength <= 0) break;

                    byte[] msgBuffer = new byte[msgLength];
                    if (await stream.ReadAsync(msgBuffer, 0, msgLength) < msgLength) break;

                    var networkMsg = JsonSerializer.Deserialize<NetworkMessage>(Encoding.UTF8.GetString(msgBuffer));

                    if (networkMsg.Type == MessageType.PlayerAction)
                    {
                        try
                        {
                            var payload = networkMsg.GetPayload<PlayerAction>();
                            if (payload != null)
                            {
                                payload.PlayerId = clientId;
                                _controller.AddLog($"[Server] Received action from {clientId}: {payload.Type} - {payload.Payload}");
                                EnqueueAction(payload);
                            }

                        }
                        catch (JsonException jsonEx)
                        {
                            _controller.errors.Add($"[Server] Invalid JSON from client {clientId}: {jsonEx.Message}");
                        }
                        catch (Exception ex)
                        {
                            _controller.errors.Add($"[Server] Error processing message from {clientId}: {ex.Message}");
                            break;
                        }
                    }
                }
            }
            catch { /* Ignoruj */ }
            finally
            {
                DisconnectPlayer(clientId);
            }
        }

        private void BroadcastBoardState()
        {
            List<TcpClient> currentClients;
            lock (_lock) { currentClients = _clients.ToList(); }
            // _controller.PrintServerMaze();
            foreach (var client in currentClients)
            {
                int clientid = _clientIds[client];
                var dto = _controller.gameState.Maze.ToDto(clientid, _controller.instruction);
                var msg = NetworkMessage.Create(MessageType.BoardUpdate, dto);
                _ = SendMessageAsync(client, msg);
            }
        }

        private async Task SendMessageAsync(TcpClient client, NetworkMessage message)
        {

            if (client == null || !client.Connected)
            {
                _controller.errors.Add($"[Server] Attempted to send message to a disconnected client.");
                return;
            }
            NetworkStream stream = null;

            try
            {

                stream = client.GetStream();
                if (!stream.CanWrite)
                {
                    _controller.errors.Add($"[Server] Stream is not writable for client {((IPEndPoint)client.Client.RemoteEndPoint).Port}.");
                    DisconnectPlayer(_clientIds[client]);
                    return;
                }

                var jsonBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                var lengthPrefix = BitConverter.GetBytes(jsonBytes.Length);

                await stream.WriteAsync(lengthPrefix, 0, lengthPrefix.Length);
                await stream.WriteAsync(jsonBytes, 0, jsonBytes.Length);
            }
            catch (ObjectDisposedException ode)
            {
                _controller.errors.Add($"[Server] Object disposed when sending message to client. {ode.Message}");
                if (_clientIds.TryGetValue(client, out int clientId))
                {
                    DisconnectPlayer(clientId);
                }
            }
            catch (Exception ex)
            {
                _controller.errors.Add($"[Server] Error sending message: {ex.Message}");
                if (_clientIds.TryGetValue(client, out int clientId))
                {
                    DisconnectPlayer(clientId);
                }
            }
        }

        private void ResetTurnFlags()
        {
            var keys = _playerHasActed.Keys.ToList();
            foreach (var key in keys)
            {
                _playerHasActed[key] = false;
            }
        }


        private void DisconnectPlayer(int playerId)
        {
            TcpClient? clientToRemove = null;

            lock (_lock)
            {
                foreach (var kvp in _clientIds)
                {
                    if (kvp.Value == playerId)
                    {
                        clientToRemove = kvp.Key;
                        break;
                    }
                }

                if (clientToRemove != null)
                {
                    _clients.Remove(clientToRemove);
                    _clientIds.Remove(clientToRemove);
                    _playerHasActed.Remove(playerId);
                }

                _controller.gameState.RemovePlayer(playerId);
                _controller.AddLog($"Client {playerId} disconnected.");
            }

            try
            {
                if (clientToRemove?.Connected == true)
                {
                    clientToRemove.Close();
                }
                else
                {
                    clientToRemove?.Dispose();
                }
            }
            catch (Exception ex)
            {
                _controller.errors.Add($"[Server] Error closing connection for player {playerId}: {ex.Message}");
            }

            BroadcastBoardState();
        }
    }

    public enum MessageType
    {
        PlayerAction,
        AssignIdAndBoard,
        BoardUpdate,
        EndOfTheGame,
        Error
    }

    public class AssignIdAndBoardPayload
    {
        public int ClientId { get; set; }
        public MazeDto InitialBoard { get; set; }
        public string instructions { get; set; }

        public int Strategy { get; set; }
    }

    public class NetworkMessage
    {
        public MessageType Type { get; set; }
        public string Payload { get; set; }

        public T GetPayload<T>()
        {
            return JsonSerializer.Deserialize<T>(Payload);
        }

        public static NetworkMessage Create<T>(MessageType type, T payload)
        {
            return new NetworkMessage
            {
                Type = type,
                Payload = JsonSerializer.Serialize(payload)
            };
        }
    }
}
