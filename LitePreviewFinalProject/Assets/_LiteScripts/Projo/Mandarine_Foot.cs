using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mandarine_Foot : AProjo
{

    public override void Start()
    {
        base.Start();
        base.FootStart();
    }


    void Update()
    {
        FootSelection();
        HighLight();
        ChangeHeight();
        MoveLight();
        FadeSound();
        RemindTransformGachette();

    }
}
