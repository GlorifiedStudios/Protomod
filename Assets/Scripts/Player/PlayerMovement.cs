
using UnityEngine;

namespace Protomod.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 6f;
        [SerializeField] private AudioClip[] footstepSounds = null;
        [SerializeField] private AudioSource footstepSource = null;

        private CharacterController charController;
        private bool walking = false;

        private void Start()
        {
            if( footstepSource == null || footstepSounds == null ) { return; }
            InvokeRepeating( "FootstepSounds", 0, 0.5f );
        }

        private void Awake()
        {
            charController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            MovementLogic();
        }

        private void MovementLogic()
        {
            float horizontal = Input.GetAxis( "Horizontal" ) * movementSpeed;
            float vertical = Input.GetAxis( "Vertical" ) * movementSpeed;
            if( ConsoleController.consoleActive ) { horizontal = 0; vertical = 0; }

            Vector3 forwardMovement = transform.forward * vertical;
            Vector3 rightMovement = transform.right * horizontal;

            Vector3 speed = forwardMovement + rightMovement;
            charController.SimpleMove( speed );
            walking = speed != Vector3.zero;
        }

        void FootstepSounds()
        {
            if( walking )
            {
                footstepSource.clip = footstepSounds[Random.Range( 0, footstepSounds.Length )];
                footstepSource.Play();
            }
        }
    }
}