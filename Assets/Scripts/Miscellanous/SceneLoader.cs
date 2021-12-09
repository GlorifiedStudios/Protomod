using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Awake()
    {
        if( Application.isBatchMode ) { // check if the build is a server/headless build
            SceneManager.LoadScene( "Server" );
        } else {
            SceneManager.LoadScene( "MainMenu" );
        }
    }
}
