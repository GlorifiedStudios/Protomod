using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
    [SerializeField] private int framerateLimit = 67;
    
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = framerateLimit;
    }
}
