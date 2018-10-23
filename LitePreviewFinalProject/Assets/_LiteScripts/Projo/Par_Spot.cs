using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Par_Spot : AProjo
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        base.ParStart();
    }


    void Update()
    {
        RotatePar();
        ChangeIntensity();
        HighLight();
        RemindTransformGachette();
    }
}
