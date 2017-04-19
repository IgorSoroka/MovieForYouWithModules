using System.Net;

namespace ModuleMainModule.Model
{
    class NetworkClient
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://wwww.google.com"))
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


        //public static bool IsSocketConnected()
    }
}
