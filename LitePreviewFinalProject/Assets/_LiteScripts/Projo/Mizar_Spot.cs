using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Mizar_Spot : AProjo {

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        isMizar = true;
        base.Start();
        base.LightStart();
        p_Outline = _GameObject.transform.Find("Foot/Height/Axe/Outliner_1").GetComponent<MeshRenderer>().materials;
        Material[] Outline = _GameObject.transform.Find("Foot/Height/Axe/Outliner_2").GetComponent<MeshRenderer>().materials;
        p_Outline = p_Outline.Concat(Outline).ToArray();
    }


    void Update()
    {
        RotateLight();
        ChangeIntensity();
        HighLight();
        RemindTransformGachette();
    }
}
