using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Model_Boutton : AModel {

    protected override void Start()
    {
        base.Start();

        p_ElevatorAnimator = GameObject.Find("Elevator").GetComponent<Animator>();
        _MeshRenderer = _Transform.GetChild(0).GetComponent<MeshRenderer>();
        p_SceneScreen = GameObject.Find("SceneScreen");
        p_SceneScreen.SetActive(false);     

        LateStart();

        Material[] Outline = transform.GetChild(2).GetComponent<MeshRenderer>().materials;
        p_Outline = p_Outline.Concat(Outline).ToArray();

    }



    void Update()
    {
        HightLightAll();
        PlayAnime();
        PlayElevator();
        RemindTransformGachette();
    }
}
