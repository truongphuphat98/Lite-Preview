using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mizar_Flap : AProjo {


    public override void Start()
    {
        base.Start();
        base.FlapStart();
        isMizar = true;
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
