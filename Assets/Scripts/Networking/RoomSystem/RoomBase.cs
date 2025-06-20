using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Networking.RoomSystem {
    
    public abstract class RoomBase {
        
        public abstract string OtherPlayerIp { get; protected set; }

        public static Byte[] ToByte(RoomInfo roominfo) {
            
            var json = JsonConvert.SerializeObject(roominfo);
            return Encoding.UTF8.GetBytes(json);
        }

        public static RoomInfo ToData(Byte[] data) {
            
            var json = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<RoomInfo>(json);
        }
        
        public static string GetIP() {
            var hostName = Dns.GetHostName();
            return Dns.GetHostEntry(hostName).AddressList[1].ToString();
        }
    }
}