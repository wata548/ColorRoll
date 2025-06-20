namespace Networking.RoomSystem {
    
    public enum RoomFindCommand {
        RoomRequest, 
        RoomInfo,
        JoinRequest,
        Allow
    }
    
    public class RoomInfo {
        public RoomFindCommand Command;
        public string Ip;
        public int Port;
        public string Name;

        public RoomInfo(RoomFindCommand command, string ip, int port, string name = "") {
            Command = command;
            Ip = ip;
            Port = port;
            Name = name;
        }
    }

}