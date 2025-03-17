using Autofac;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.ServerDataInfo;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Server;

internal class ServerCore
{
    private string host;
    private int port;
    private TcpListener listener;
    private List<Client> clients = new List<Client>();
    
    ActiveConnectionsManager connectionsManager { get; set; }
    Db db = new Db();
    public ServerCore(string host = "127.0.0.1", int port = 5500)
    {
        this.host = host;
        this.port = port;

        listener = new TcpListener(IPAddress.Parse(host), port);

        var rooms = db.Rooms.Include(u => u.Users).ToList();


        connectionsManager = new ActiveConnectionsManager(rooms);
    }

    public async Task StartAsync()
    {
        try
        {
            listener.Start();
            await Console.Out.WriteLineAsync($"Server started at {host}:{port}");

            while (true)
            {
                TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected...");

                Client client = new Client(tcpClient, connectionsManager);

                // TODO: Add: client handlers
              /*  User user = new User()
                {
                    Login = "TestUser",
                    Password = "lawleal"
                };
                using(Db db = new Db())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }*/
                clients.Add(client);
                connectionsManager.AddConectedClient(client);
                
                _ = Task.Run(() => client.Processing());
            }
        }
        catch (Exception ex)
        {
            // TODO: mock. Add error handling
            await Console.Out.WriteLineAsync($"ERROR: {ex.Message}");
        }
    }
}
