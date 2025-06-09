using Game2.Model;
using System.Net.NetworkInformation; // This using directive isn't used in this file. Can be removed if not used elsewhere.

namespace Game2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string mode = "";
            int port = 5555; // Domyślny port dla serwera i klienta
            string address = "127.0.0.1"; // Domyślny adres serwera dla klienta

            if (args.Length > 0)
            {
                if (args[0].ToLower() == "--server")
                {
                    mode = "server";
                    if (args.Length > 1 && int.TryParse(args[1], out int pSrv)) port = pSrv;
                }
                else if (args[0].ToLower() == "--client")
                {
                    mode = "client";
                    if (args.Length > 1)
                    {
                        string clientArg = args[1];
                        if (clientArg.Contains(':'))
                        {
                            var parts = clientArg.Split(':');
                            address = parts[0];
                            if (parts.Length > 1 && int.TryParse(parts[1], out int pCli)) port = pCli;
                        }
                        else
                        {
                            address = clientArg;
                        }
                    }
                    if (args.Length > 2 && !args[1].Contains(':') && int.TryParse(args[2], out int pCliArgs))
                    {
                        port = pCliArgs;
                    }
                }
            }
            else
            {
                if (!GameRendererS.ChooseTcp(ref mode, ref port, ref address)) return;
            }

            if (mode == "server")
            {
                //Console.WriteLine($"Starting server on port {port}...");
                var server = new Model.GameServer(port);
                await server.StartAsync(); // This call blocks until the server listener stops (e.g., on error or external stop)
                // Console.WriteLine("Server application finished."); // This will be hit if StartAsync completes
            }
            else if (mode == "client")
            {
                Console.WriteLine($"Starting client connecting to {address}:{port}...");
                var client = new Client();
                await client.StartAsync(address, port); // This call blocks until client disconnects or an error occurs
                Console.WriteLine("Client application finished. Press any key to exit."); // This will be hit if StartAsync completes
                Console.ReadKey(); // Keep the console window open until a key is pressed.
            }
            else
            {
                Console.WriteLine("No mode selected or invalid arguments. Use --server [port] or --client [address:port]");
            }
        }
    }
}