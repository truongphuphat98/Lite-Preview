using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Par_Foot : AProjo {


    public override void Start()
    {
        isPar = true;
        base.Start();
        base.FootStart();
    }


    void Update()
    {
        FootSelection();
        HighLight();
        MoveLight();
        FadeSound();
        RemindTransformGachette();
    }
}
