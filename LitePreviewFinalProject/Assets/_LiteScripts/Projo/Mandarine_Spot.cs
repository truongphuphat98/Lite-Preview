using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mandarine_Spot : AProjo
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        base.LightStart();
    }


    void Update()
    {
        RotateLight();
        ChangeIntensity();
        HighLight();
        FadeSound();
        RemindTransformGachette();
    }
}
