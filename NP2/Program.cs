using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NP2;

class Program
{
    const int Port = 8888;
    static void Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Loopback, Port);
        server.Start();
        Console.WriteLine($"Listening on port {Port}");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected");
            
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int byteCount = stream.Read(buffer, 0, buffer.Length);
            string requestedCode = Encoding.UTF8.GetString(buffer, 0, byteCount);
            Console.WriteLine($"Received request for {requestedCode}");
            
            List<string> streets = FindStreetsForCode(requestedCode);
            
            string response = streets.Count > 0 ? string.Join("\n", streets) : "No streets found"; 
            byte[] responseData = Encoding.UTF8.GetBytes(response);
            stream.Write(responseData, 0, responseData.Length);
            
            client.Close();
            Console.WriteLine("Closing connection");
        }
    }

    static List<string> FindStreetsForCode(string code)
    {
        var lines = File.ReadAllLines("streets.txt");
        List<string> streets = new List<string>();
        bool isCorrectSection = false;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line.Trim() == code)
            {
                isCorrectSection = true;
                continue;
            }
            else if (line.Trim().All(char.IsDigit))
            {
                isCorrectSection = true;
            }

            if (isCorrectSection)
            {
                streets.Add(line.Trim());
            }
        }
        return streets;
    }
}