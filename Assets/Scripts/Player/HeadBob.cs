using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private float bobbingSpeed = 0.18f;
    [SerializeField] private float bobbingAmount = 0.2f;

    private float midpoint = 0.0f;
    private float timer = 0.0f;

    void Awake() {
        midpoint = transform.localPosition.y;
    }

    void Update()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis( "Horizontal" );
        float vertical = Input.GetAxis( "Vertical" );
        if( ConsoleController.consoleActive ) { horizontal = 0; vertical = 0; }

        Vector3 cSharpConversion = transform.localPosition;

        if( Mathf.Abs( horizontal ) == 0 && Mathf.Abs( vertical ) == 0 ) {
            timer = 0.0f;
        } else {
            waveslice = Mathf.Sin( timer );
            timer = timer + bobbingSpeed;
            if( timer > Mathf.PI * 2 )
            {
                timer = timer - ( Mathf.PI * 2 );
            }
        }

        if ( waveslice != 0 ) {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs( horizontal ) + Mathf.Abs( vertical );
            totalAxes = Mathf.Clamp( totalAxes, 0.0f, 1.0f );
            translateChange = totalAxes * translateChange;
            cSharpConversion.y = midpoint + translateChange;
        } else {
            cSharpConversion.y = midpoint;
        }

        transform.localPosition = cSharpConversion;
    }
}
