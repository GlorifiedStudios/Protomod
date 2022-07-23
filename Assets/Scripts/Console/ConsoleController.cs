
using System;
using TMPro;
using UnityEngine;

namespace Protomod
{
    public class ConsoleController : MonoBehaviour
    {
        public static bool consoleActive;

        public static void AddLineToConsole( string newText )
        {
        }

        public static void ThrowError( string errorText ) {  }
        public static void ThrowWarning( string warningText ) {  }
        public static void PrintToConsole( string printText ) {  }

        void ToggleConsole()
        {
            consoleActive = !consoleActive;

            if( consoleActive )
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        void Update()
        {
            if( Input.GetKeyDown( KeyCode.Tilde ) )
                ToggleConsole();
        }
    }
}