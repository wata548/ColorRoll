namespace Networking.RoomSystem {
    
    public enum RoomCommand {
        RoomRequest, 
        RoomInfo,
        JoinRequest,
        Allow,
        Quit,
        Start
    }
    
    public class RoomInfo {
        public RoomCommand Command;
        public string Ip;
        public int Port;
        public string Name;

        public RoomInfo(RoomCommand command, string ip, int port, string name = "") {
            Command = command;
            Ip = ip;
            Port = port;
            Name = name;
        }
    }

}