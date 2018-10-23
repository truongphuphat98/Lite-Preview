using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AModel : MonoBehaviour
{

    public bool isTarget;
	public float outlineSize = 8;
    protected bool isTele;
    protected bool animeplayed;
    protected bool elevatorplayed;
    [SerializeField]
    protected Material[] p_Outline;
    protected Transform _Transform;
    protected MeshRenderer _MeshRenderer;
    public bool _InputGachetteR = false, _InputGachetteL = false;
    [SerializeField]
    protected Animator _Animator;
    protected Animator p_ElevatorAnimator;
    protected GameObject p_CameraRend, p_ShadowRend, p_SceneScreen;
    protected float rVrY, rVrX, rVrZ, lVrY, lVrX, lVrZ;
    protected Transform p_GachetteRemindL, p_GachetteTransformL, p_GachetteRemindR, p_GachetteTransformR;
    protected bool p_remindGachetteL, p_remindGachetteR, p_CheckRClick;
    protected GameObject _left, _right;
	public AudioClip openCase;

    protected virtual void Start()
    {
        _Transform = transform;
    }

    protected void LateStart()
    {
        _Animator = _Transform.GetComponent<Animator>();
        p_Outline = _MeshRenderer.materials;
    }

    protected void ActiveTele()
    {
        if (isTarget && !isTele && _InputGachetteR && p_GachetteRemindR != null)
        {
            isTele = true;
            p_ShadowRend.SetActive(true);
            p_CameraRend.SetActive(true);
        }
    }

    protected void HighLight()
    {
        if (isTarget)
        {
			p_Outline[1].SetFloat("_Outline", outlineSize);
        }
        else
        {
            p_Outline[1].SetFloat("_Outline", -0.1f);
        }
    }

    protected void HightLightAll()
    {
        if(isTarget)
        {
			p_Outline[1].SetFloat("_Outline", outlineSize);
			p_Outline[3].SetFloat("_Outline", outlineSize);
        }
        else
        {
            p_Outline[1].SetFloat("_Outline", -0.1f);
            p_Outline[3].SetFloat("_Outline", -0.1f);
        }
    }

	protected void PlayAudio()
	{
		GetComponent<AudioSource>().PlayOneShot(openCase);
	}

    protected void PlayElevator()
    {
        if (animeplayed && !elevatorplayed)
        {
            p_SceneScreen.SetActive(true);
            elevatorplayed = true;
            p_ElevatorAnimator.SetBool("Play", true);
        }
    }

    protected void PlayAnime()
    {
        if (isTarget && _InputGachetteR && p_GachetteRemindR != null && !animeplayed)
        {
			if (openCase != null) {
				PlayAudio ();
			}
            animeplayed = true;
            _Animator.SetBool("Play", true);
        }
    }

    protected void RemindTransformGachette()
    {
        if (_left == null)
            _left = GameObject.FindGameObjectWithTag("Left");
        if (_right == null)
            _right = GameObject.FindGameObjectWithTag("Right");

        if (isTarget && _InputGachetteL)
        {
            p_GachetteTransformL = _left.transform;
        }

        if (isTarget && _InputGachetteL && !p_remindGachetteL)
        {
            p_remindGachetteL = true;
            p_GachetteRemindL = _left.transform;
            lVrY = _left.transform.position.y;
            lVrX = _left.transform.position.x;
            lVrZ = _left.transform.position.z;
        }
        if (!_InputGachetteL)
        {
            p_remindGachetteL = false;
            p_GachetteRemindL = null;
            p_GachetteTransformL = null;
        }

        if (isTarget && _InputGachetteR)
        {
            p_GachetteTransformR = _right.transform;
        }

        if (isTarget && _InputGachetteR && !p_remindGachetteR)
        {
            p_remindGachetteR = true;
            p_GachetteRemindR = _right.transform;
            rVrY = _right.transform.position.y;
            rVrX = _right.transform.position.x;
            rVrZ = _right.transform.position.z;
        }
        if (!_InputGachetteR)
        {
            p_CheckRClick = false;
            p_remindGachetteR = false;
            p_GachetteRemindR = null;
            p_GachetteTransformR = null;
        }
    }
}
