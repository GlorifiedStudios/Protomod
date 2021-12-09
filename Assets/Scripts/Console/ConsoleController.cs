
using System;
using TMPro;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{
    public static bool consoleActive;
    private static string consoleText = "";
    private static TMP_InputField consoleTextInputField = null;

    [SerializeField] private GameObject consoleGameObject = null;
    [SerializeField] private TMP_InputField consoleInput = null;

    public static void AddLineToConsole( string newText )
    {
        if( consoleTextInputField == null ) { return; }
        consoleText = consoleText + newText + "\n";
        consoleTextInputField.text = consoleText;
    }

    public static void ThrowError( string errorText ) { AddLineToConsole( "<color=red>" + errorText + "</color>" ); }
    public static void ThrowWarning( string warningText ) { AddLineToConsole( "<color=orange>" + warningText + "</color>" ); }
    public static void PrintToConsole( string printText ) { AddLineToConsole( "<color=white>" + printText + "</color>" ); }

    public static void ThrowExceptionToConsole( Exception ex )
    {
        Debug.Log( ex );
        string niceMessage = ex.Message;
        niceMessage = niceMessage.Replace( @"\", "/" );
        int modulesIndex = niceMessage.IndexOf( "modules" );
        niceMessage = niceMessage.Substring( modulesIndex, niceMessage.Length - modulesIndex );
        ThrowError( niceMessage );
    }

    void Awake()
    {
        consoleActive = consoleGameObject.activeSelf;
        consoleTextInputField = consoleGameObject.GetComponent<TMP_InputField>();
    }

    void OnGUI()
    {
        if( consoleInput.isFocused && consoleInput.text != "" && Input.GetKey( KeyCode.Return ) )
        {
            PrintToConsole( consoleInput.text );
            consoleInput.text = "";
        }
    }

    void ToggleConsole()
    {
        consoleGameObject.SetActive( !consoleActive );
        consoleActive = consoleGameObject.activeSelf;

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
        if( Input.GetKeyDown( KeyCode.F1 ) )
            ToggleConsole();
    }
}