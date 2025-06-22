using UnityEngine;

namespace Networking.InGame {
    public class GameManager: MonoBehaviour {

        [Header("Host")] 
        [SerializeField] private GameObject _hostPlayer1Prefab;
        [SerializeField] private GameObject _hostPlayer2Prefab; 
        [Space]
        [Header("Client")] 
        [SerializeField] private GameObject _clientPlayer1Prefab;
        [SerializeField] private GameObject _clientPlayer2Prefab; 
        
        
    }
}