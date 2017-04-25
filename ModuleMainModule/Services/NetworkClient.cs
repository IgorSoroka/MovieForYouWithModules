using System.Net;
using System.Net.Sockets;

namespace ModuleMainModule.Services
{
    public class NetworkClient
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.themoviedb.org/"))
                    {
                        return true;
                    }
                }
            }
            catch 
            {
                return false;
            }
        }       
    }

    public static class SocketExtensions
    {
        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        public static bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 & part2)
            {
                return false;
            }
            return true;
        }
    }
}