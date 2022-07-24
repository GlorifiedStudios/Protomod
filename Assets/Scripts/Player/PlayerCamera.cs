
using UnityEngine;

namespace Protomod.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 120f;

        [SerializeField] private Transform playerBody = null;

        private float xAxisClamp;

        private void Awake()
        {
            xAxisClamp = 0.0f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; // TODO: Make a less confusing cursor handler, because it's being called from this script and ConsoleController.
        }

        private void Update()
        {
            if( Console.consoleActive ) { return; }
            CameraRotation();
        }

        private void CameraRotation()
        {
            float mouseX = Input.GetAxis( "Mouse X" ) * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis( "Mouse Y" ) * mouseSensitivity * Time.deltaTime;

            xAxisClamp += mouseY;

            if( xAxisClamp > 90.0f )
            {
                xAxisClamp = 90.0f;
                mouseY = 0.0f;
                ClampXAxisRotationToValue( 270.0f );
            } else if( xAxisClamp < -90.0f )
            {
                xAxisClamp = -90.0f;
                mouseY = 0.0f;
                ClampXAxisRotationToValue( 90.0f );
            }

            transform.Rotate( Vector3.left * mouseY );
            playerBody.Rotate( Vector3.up * mouseX );
        }

        private void ClampXAxisRotationToValue( float value )
        {
            Vector3 eulerRotation = transform.eulerAngles;
            eulerRotation.x = value;
            transform.eulerAngles = eulerRotation;
        }
    }
}