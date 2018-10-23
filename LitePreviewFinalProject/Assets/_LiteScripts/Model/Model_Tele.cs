using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Tele : AModel
{


    protected override void Start()
    {
        base.Start();
        p_CameraRend = GameObject.Find("CameraRenderer");
        p_ShadowRend = GameObject.Find("ShadowRenderer");
        _MeshRenderer = _Transform.GetChild(0).GetComponent<MeshRenderer>();

        LateStart();
        p_ShadowRend.SetActive(false);
        p_CameraRend.SetActive(false);
    }



    void Update()
    {

        RemindTransformGachette();
        if (isTarget)
        {
            p_Outline[0].SetFloat("_Outline", outlineSize);
        }
        else
        {
            p_Outline[0].SetFloat("_Outline", -0.5f);
        }

        ActiveTele();
    }
}
