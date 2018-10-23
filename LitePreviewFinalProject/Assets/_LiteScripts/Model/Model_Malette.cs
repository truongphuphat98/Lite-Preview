using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Malette : AModel {

    protected override void Start()
    {
        base.Start();

        _MeshRenderer = _Transform.GetChild(2).GetComponent<MeshRenderer>();
        _Transform = _Transform.parent;

        LateStart();
    }



    void Update()
    {
        HighLight();
        PlayAnime();
        RemindTransformGachette();
    }
}
