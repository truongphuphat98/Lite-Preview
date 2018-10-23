using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mandarine_Flap : AProjo
{


    public override void Start()
    {
        isMizar = false;
        base.Start();
        base.FlapStart();
    }


    void Update()
    {
        RotateFlap();
        HighLight();
        LightSpotAngle();
        RemindTransformGachette();
        ChangeColor();
    }
}
