
using UnityEngine;

namespace Protomod.Player
{
    public class PlayerReferences : MonoBehaviour
    {
        public static PlayerReferences Instance;
        public Rigidbody Rigidbody;
        public CharacterController CharacterController;
        public PlayerMovement PlayerMovement;
        public PlayerCamera PlayerCamera;
        public PlayerHeadBob PlayerHeadBob;

        private void Start() => Instance = this;
    }
}
