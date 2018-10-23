using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mizar_Foot : AProjo {


    public override void Start()
    {
        isMizar = true;
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
