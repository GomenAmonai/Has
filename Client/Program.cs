using System.Net.Sockets;
using System.Text;

namespace Client;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите почтовый индекс : ");
        string postalCode = Console.ReadLine();
        
        TcpClient client = new TcpClient();
        client.Connect("127.0.0.1", 8888);
        NetworkStream stream = client.GetStream();
        
        byte[] data = Encoding.UTF8.GetBytes(postalCode);
        stream.Write(data, 0, data.Length);
        
        byte[] buffer = new byte[1024];
        int byteCount = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.UTF8.GetString(buffer, 0, byteCount);

        Console.WriteLine("Названия улиц: ");
        Console.WriteLine(response);
        
        client.Close();
    }
}