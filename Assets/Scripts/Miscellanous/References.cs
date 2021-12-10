
using UnityEngine;

namespace Protomod
{
    public class References : MonoBehaviour
    {
        public static References Instance;
        public GameObject Player;

        private void Start() => Instance = this;
    }
}
