using System.Net;

namespace Networking.RoomSystem {
    
    public abstract class RoomBase {
        
        public static string GetIP() {
            var hostName = Dns.GetHostName();
            return Dns.GetHostEntry(hostName).AddressList[1].ToString();
        }
    }
}