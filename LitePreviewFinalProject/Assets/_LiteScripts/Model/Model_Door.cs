using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class Model_Door : AModel
{
    public bool startScene;
    GameObject outliner1, outliner2;
    public AudioClip openDoor;

    protected override void Start()
    {
        base.Start();


        outliner1 = _Transform.GetChild(2).gameObject;
        outliner2 = _Transform.GetChild(3).gameObject;

        _Animator = _Transform.GetComponent<Animator>();

        outliner1.SetActive(false);
        outliner2.SetActive(false);
    }



    void Update()
    {
        RemindTransformGachette();
        if (isTarget)
        {
            outliner1.SetActive(true);
            outliner2.SetActive(true);
        }
        else if (outliner1 && outliner2)
        {
            outliner1.SetActive(false);
            outliner2.SetActive(false);
        }

        if (isTarget && _InputGachetteR && p_GachetteRemindR != null && !p_CheckRClick)
        {
            p_CheckRClick = true;
            GetComponent<AudioSource>().PlayOneShot(openDoor);
            _Animator.SetBool("Play", true);            
        }

        if (startScene)
        {
            SceneManager.LoadScene(0);
        }
    }
}
