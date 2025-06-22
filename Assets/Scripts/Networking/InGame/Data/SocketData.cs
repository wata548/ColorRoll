using Newtonsoft.Json;

namespace Networking.InGame {

    public enum DataType {
        Input,
        CurState,
        GameEnd,
        Result
    }
    
    public class SocketData {

        public DataType DataType;
        public string Data;
        
        public SocketData(IData data) {

            DataType = data switch {
                GameData => DataType.CurState,
                InputData => DataType.Input
            };
            Data = JsonConvert.SerializeObject(data);
        }

    }
}