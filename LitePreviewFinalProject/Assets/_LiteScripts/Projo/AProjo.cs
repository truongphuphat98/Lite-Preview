using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.AI;

public abstract class AProjo : MonoBehaviour
{
    //SteamVR_Controller.Device Controller;
    SteamVR_TrackedObject trackedObj;

    public bool isTarget = false;
    protected bool isPar = false;
    protected bool isMizar = false;
    protected bool isFlap = false;
    protected bool p_CanRotate = false;
    protected bool p_CanMove = true;
    protected bool p_destinationReach = true;
    protected bool p_FadeSound = false;
    protected bool p_InstantiateCircle = false;
    public bool _InputGachetteR = false, _InputGachetteL = false;
    protected bool p_remindGachetteR = false, p_remindGachetteL = false;
    protected bool p_CheckLClick = false, p_CheckRClick = false;
    [SerializeField]
    protected bool p_TouchedColor = false;
    protected bool p_Pressed = false;

    protected GameObject _TransformSpot;
    protected GameObject _SpotDeco;
    protected GameObject _TransformAxe;
    protected GameObject _GameObject;
    protected GameObject _TransformCircle;
    protected GameObject p_CircleInstance;
    protected GameObject height;
    protected GameObject _FlapD;
    protected GameObject _FlapL;
    protected GameObject _FlapU;
    protected GameObject _FlapR;
    protected GameObject _left, _right;
    protected NavMeshAgent _NavProjo;
    protected AudioSource _AudioSource;
    public AudioClip _IntensitySound, _RotationSound;
    protected Vector3 p_targetMove;

    public Vector2 p_ColorVector;
    //public Color[] lightColor;
    protected Transform p_GachetteTransformL, p_GachetteRemindL, p_GachetteTransformR, p_GachetteRemindR;

    protected Transform _Transform;
    protected Light _light;
    [SerializeField]
    protected Material[] p_Outline;

    public float outlineSize = 0.05f, outlineOffSize = -0.01f;
    public float minHeight = 0.0548f;
    public float maxHeight = 0.096f;
    protected float rVrY, rVrX, rVrZ, lVrY, lVrX, lVrZ;



    [System.Serializable]
    public class Lights
    {
        public float initRange = 30;
        public float initSpotAngle = 45;
        public float initMinSpotAngle = 10;
        public float initMaxSpotAngle = 90;
        public int CurrentIntensity = 1;
        public float[] initIntensities = { 0.2f, 0.5f, 1f, 1.5f, 2f };
    }
    public Lights lights;

    [System.Serializable]
    public class Projo
    {
        public float initMinZRot = 22f;
        public float initMaxZRot = -35f;
    }
    public Projo _Projo;

    protected float p_Range;
    protected float p_SpotAngle;
    protected float p_MinSpotAngle;
    protected float p_MaxSpotAngle;
    protected int p_CurrentIntensity;
    protected float[] p_Intensities;

    [Range(5f, 100f)]
    public float range;

    protected float horizontalSpeed;
    [SerializeField]
    protected float verticalSpeed;

    public virtual void Awake()
    {
    }

    public virtual void Start()
    {
        _Transform = transform;
    }

    public virtual void LightStart()
    {
        _AudioSource = GetComponent<AudioSource>();
        _GameObject = _Transform.parent.parent.parent.parent.gameObject;
        _TransformSpot = _GameObject.transform.Find("Foot/Height/Axe/Spot").gameObject;
        _SpotDeco = _GameObject.transform.Find("Foot/Height/Axe/Deco").gameObject;
        _TransformAxe = _GameObject.transform.Find("Foot/Height/Axe").gameObject;
        p_Range = lights.initRange;
        _light = _GameObject.GetComponentInChildren<Light>();
        p_CurrentIntensity = lights.CurrentIntensity;
        p_Intensities = lights.initIntensities;
        _light.intensity = p_Intensities[p_CurrentIntensity];
        if (!isMizar)
            p_Outline = _TransformSpot.GetComponent<MeshRenderer>().materials;
    }

    public virtual void ParStart()
    {
        _AudioSource = GetComponent<AudioSource>();
        p_Range = lights.initRange;
        _light = GetComponentInChildren<Light>();
        p_CurrentIntensity = lights.CurrentIntensity;
        p_Intensities = lights.initIntensities;
        _light.intensity = p_Intensities[p_CurrentIntensity];
        p_Outline = GetComponent<MeshRenderer>().materials;
    }

    public virtual void FootStart()
    {
        _AudioSource = GetComponent<AudioSource>();
        if (!isPar)
            _NavProjo = GetComponent<NavMeshAgent>();
        if (isPar)
            _NavProjo = _Transform.parent.GetComponent<NavMeshAgent>();
        _GameObject = _Transform.parent.gameObject;
        _TransformCircle = transform.Find("TransStart").gameObject;
        _TransformCircle.SetActive(false);
        if (!isPar)
        {
            height = _Transform.transform.Find("Height").gameObject;
        }
        p_Outline = GetComponent<MeshRenderer>().materials;
    }

    protected virtual void FlapStart()
    {
        _FlapD = transform.Find("D").gameObject;
        _FlapL = transform.Find("L").gameObject;
        _FlapR = transform.Find("R").gameObject;
        _FlapU = transform.Find("U").gameObject;
        _GameObject = _Transform.parent.parent.parent.parent.parent.gameObject;
        _light = _GameObject.GetComponentInChildren<Light>();
        p_SpotAngle = lights.initSpotAngle;
        p_MinSpotAngle = lights.initMinSpotAngle;
        p_MaxSpotAngle = lights.initMaxSpotAngle;
        isFlap = true;
        for (int i = 0; i < 4; i++)
        {
            Material[] Outline = transform.GetChild(i).GetComponent<MeshRenderer>().materials;
            p_Outline = p_Outline.Concat(Outline).ToArray();
        }
    }

    protected void FootSelection()
    {
        if (isTarget && !p_InstantiateCircle && _InputGachetteR && p_GachetteRemindR != null)
        {
            _TransformCircle.SetActive(true);
            GameObject circleClone = Instantiate(_TransformCircle, _TransformCircle.transform.position, _TransformCircle.transform.rotation) as GameObject;
            circleClone.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            p_CircleInstance = circleClone;
            p_InstantiateCircle = true;
        }

        if (Vector3.Distance(new Vector3(_Transform.position.x, 0, _Transform.position.z), new Vector3(p_targetMove.x, 0, p_targetMove.z)) < 0.1f && p_destinationReach == false)
        {
            p_destinationReach = true;
            p_FadeSound = true;
        }
        if (p_CircleInstance != null && p_InstantiateCircle && !_InputGachetteR && p_GachetteRemindR == null)
        {
            _AudioSource.Play();
            _NavProjo.SetDestination(p_CircleInstance.transform.position);
            _TransformCircle.SetActive(false);
            p_targetMove = p_CircleInstance.transform.position;
            Destroy(p_CircleInstance);
            p_InstantiateCircle = false;
            p_destinationReach = false;
        }

    }

    protected void FadeSound()
    {
        if (p_destinationReach == true && p_FadeSound)
        {
            _AudioSource.volume -= Time.deltaTime * 2;
            if (_AudioSource.volume <= 0.05f)
            {
                _AudioSource.Stop();
                _AudioSource.volume = 1f;
                p_FadeSound = false;
            }
        }
    }

    protected void RemindTransformGachette()
    {
        if (_left == null)
            _left = GameObject.FindGameObjectWithTag("Left");
        if (_right == null)
            _right = GameObject.FindGameObjectWithTag("Right");

        if (_right != null)
        {
            trackedObj = _right.GetComponent<SteamVR_TrackedObject>();
            //Controller = SteamVR_Controller.Input((int)trackedObj.index);
        }

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
            p_CheckLClick = false;
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


    protected void ChangeHeight()
    {


        if (isTarget && _InputGachetteL && p_GachetteRemindL != null)
        {
            if (height.transform.position.y >= minHeight && height.transform.position.y <= maxHeight)
            {

                verticalSpeed = p_GachetteTransformL.position.y - lVrY;

                height.transform.position += height.transform.up * verticalSpeed * 1f;

            }
        }
        if (height.transform.position.y < minHeight)
            height.transform.position = new Vector3(height.transform.position.x, minHeight, height.transform.position.z);
        if (height.transform.position.y > maxHeight)
            height.transform.position = new Vector3(height.transform.position.x, maxHeight, height.transform.position.z);

    }

    protected void HighLight()
    {
        if (!isFlap && !isMizar)
        {
            if (isTarget)
            {
                p_Outline[1].SetFloat("_Outline", outlineSize);
            }
            else
                p_Outline[1].SetFloat("_Outline", outlineOffSize);
        }

        if (!isFlap && isMizar)
        {
            if (isTarget)
            {
                p_Outline[0].SetFloat("_Outline", outlineSize);
                p_Outline[1].SetFloat("_Outline", outlineSize);
            }
            else
            {
                p_Outline[0].SetFloat("_Outline", outlineOffSize);
                p_Outline[1].SetFloat("_Outline", outlineOffSize);
            }
        }

        if (isFlap)
        {
            if (isTarget)
            {
                p_Outline[1].SetFloat("_Outline", outlineSize);
                p_Outline[3].SetFloat("_Outline", outlineSize);
                p_Outline[5].SetFloat("_Outline", outlineSize);
                p_Outline[7].SetFloat("_Outline", outlineSize);
            }
            else
            {
                p_Outline[1].SetFloat("_Outline", outlineOffSize);
                p_Outline[3].SetFloat("_Outline", outlineOffSize);
                p_Outline[5].SetFloat("_Outline", outlineOffSize);
                p_Outline[7].SetFloat("_Outline", outlineOffSize);
            }
        }
    }

    protected void NavMove()
    {

    }

    protected void LightSpotAngle()
    {
        if (isTarget && _InputGachetteR && p_GachetteRemindR != null && _light.spotAngle >= p_MinSpotAngle && _light.spotAngle <= p_MaxSpotAngle)
        {
            verticalSpeed = p_GachetteTransformR.position.y - rVrY;
            _light.spotAngle += 5 * verticalSpeed;
        }
        if (_light.spotAngle < p_MinSpotAngle)
            _light.spotAngle = p_MinSpotAngle;
        if (_light.spotAngle > p_MaxSpotAngle)
            _light.spotAngle = p_MaxSpotAngle;
    }

    protected void ChangeIntensity()
    {
        if (isTarget && _InputGachetteL && p_GachetteRemindL != null && !p_CheckLClick)
        {
            p_CheckLClick = true;
            _AudioSource.PlayOneShot(_IntensitySound);
            p_CurrentIntensity++;
            if (p_CurrentIntensity > p_Intensities.Length - 1)
            {
                p_CurrentIntensity = 0;
            }
            _light.intensity = p_Intensities[p_CurrentIntensity];
        }
    }


    protected void CullingMask()
    {
        // _light.cullingMask = 1 << 8 | 1 << 8;
    }

    protected void MoveLight()
    {
        if (p_CircleInstance != null)
        {
            p_CircleInstance.transform.position = new Vector3(trackedObj.GetComponent<SteamVR_LaserPointer>().rayPosition.x, 0.01f, trackedObj.GetComponent<SteamVR_LaserPointer>().rayPosition.z);
        }
    }

    protected void RotateLight()
    {
        if (isTarget && _InputGachetteR && p_GachetteRemindR != null)
        {
            //_AudioSource.Play(_RotationSound);
            horizontalSpeed = p_GachetteTransformR.position.x - rVrX;
            verticalSpeed = p_GachetteTransformR.position.y - rVrY;

            if (!isMizar)
            {
                float rotz = _TransformSpot.transform.eulerAngles.z;

                if (rotz > 180)
                {
                    rotz = rotz - 360;
                }

                if (rotz <= _Projo.initMinZRot && rotz >= _Projo.initMaxZRot)
                {
                    if (rotz <= _Projo.initMaxZRot + 0.001f && verticalSpeed > 0)
                    {
                    }
                    else
                    {
                        if (rotz >= _Projo.initMinZRot - 0.001f && verticalSpeed < 0)
                        {
                        }
                        else
                        {
                            _TransformSpot.transform.rotation *= Quaternion.Euler(Vector3.forward * -verticalSpeed * 20);
                            _SpotDeco.transform.rotation *= Quaternion.Euler(Vector3.forward * -verticalSpeed * 20);
                        }
                    }
                }

                _TransformAxe.transform.rotation *= Quaternion.Euler(Vector3.up * horizontalSpeed * 10);

                if (rotz > _Projo.initMinZRot)
                {
                    _TransformSpot.transform.rotation = Quaternion.Euler(_TransformSpot.transform.eulerAngles.x, _TransformSpot.transform.eulerAngles.y, _Projo.initMinZRot);
                    _SpotDeco.transform.rotation = Quaternion.Euler(_TransformSpot.transform.eulerAngles.x, _TransformSpot.transform.eulerAngles.y, _Projo.initMinZRot);
                }
                if (rotz < _Projo.initMaxZRot)
                {
                    _TransformSpot.transform.rotation = Quaternion.Euler(_TransformSpot.transform.eulerAngles.x, _TransformSpot.transform.eulerAngles.y, _Projo.initMaxZRot);
                    _SpotDeco.transform.rotation = Quaternion.Euler(_TransformSpot.transform.eulerAngles.x, _TransformSpot.transform.eulerAngles.y, _Projo.initMaxZRot);
                }
            }
            if (isMizar)
            {

                float rotz = _TransformSpot.transform.eulerAngles.z;

                if (rotz > 180)
                {
                    rotz = rotz - 360;
                }

                if (rotz >= _Projo.initMinZRot && rotz <= _Projo.initMaxZRot)
                {
                    if (rotz >= _Projo.initMaxZRot - 0.001f && verticalSpeed > 0)
                    {
                    }
                    else
                    {
                        if (rotz <= _Projo.initMinZRot + 0.001f && verticalSpeed < 0)
                        {
                        }
                        else
                        {
                            _TransformSpot.transform.rotation *= Quaternion.Euler(Vector3.forward * verticalSpeed * 20);
                            _SpotDeco.transform.rotation *= Quaternion.Euler(Vector3.forward * verticalSpeed * 20);
                            _GameObject.transform.Find("Foot/Height/Axe/Outliner_1").transform.rotation *= Quaternion.Euler(Vector3.forward * verticalSpeed * 20);
                            _GameObject.transform.Find("Foot/Height/Axe/Outliner_2").transform.rotation *= Quaternion.Euler(Vector3.forward * verticalSpeed * 20);
                        }
                    }
                }
                _TransformAxe.transform.rotation *= Quaternion.Euler(Vector3.up * -horizontalSpeed * 10);



                if (rotz < _Projo.initMinZRot)
                {
                    _TransformSpot.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, _Projo.initMinZRot);
                    _SpotDeco.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, _Projo.initMinZRot);
                    _GameObject.transform.Find("Foot/Height/Axe/Outliner_1").transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, _Projo.initMinZRot);
                    _GameObject.transform.Find("Foot/Height/Axe/Outliner_2").transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, _Projo.initMinZRot);
                }
                if (rotz > _Projo.initMaxZRot)
                {
                    _TransformSpot.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, _Projo.initMaxZRot);
                    _SpotDeco.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, _Projo.initMaxZRot);
                    _GameObject.transform.Find("Foot/Height/Axe/Outliner_1").transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, _Projo.initMaxZRot);
                    _GameObject.transform.Find("Foot/Height/Axe/Outliner_2").transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, _Projo.initMaxZRot);
                }
            }
        }
    }

    protected void RotatePar()
    {
        if (isTarget && _InputGachetteR && p_GachetteRemindR != null)
        {
            horizontalSpeed = p_GachetteTransformR.position.x - rVrX;

            _Transform.parent.parent.rotation *= Quaternion.Euler(Vector3.up * -horizontalSpeed * 10);
        }
    }

    protected void RotateFlap()
    {
        if (isTarget && _InputGachetteR && p_GachetteRemindR != null && _light.spotAngle > p_MinSpotAngle && _light.spotAngle < p_MaxSpotAngle)
        {
            horizontalSpeed = p_GachetteTransformR.position.y - rVrY;
            if (!isMizar)
            {
                _FlapD.transform.rotation *= Quaternion.Euler(Vector3.forward * horizontalSpeed * 2);
                _FlapL.transform.rotation *= Quaternion.Euler(Vector3.up * horizontalSpeed * 2);
                _FlapR.transform.rotation *= Quaternion.Euler(Vector3.up * -horizontalSpeed * 2);
                _FlapU.transform.rotation *= Quaternion.Euler(Vector3.forward * -horizontalSpeed * 2);
            }
            if (isMizar)
            {
                _FlapD.transform.rotation *= Quaternion.Euler(Vector3.forward * -horizontalSpeed * 2);
                _FlapL.transform.rotation *= Quaternion.Euler(Vector3.up * horizontalSpeed * 2);
                _FlapR.transform.rotation *= Quaternion.Euler(Vector3.up * -horizontalSpeed * 2);
                _FlapU.transform.rotation *= Quaternion.Euler(Vector3.forward * horizontalSpeed * 2);

            }
        }
    }

    protected void ChangeColor()
    {
        if (_left)
        {
            p_TouchedColor = GameObject.FindGameObjectWithTag("Left").GetComponent<SteamVR_TrackedController>().touched;
            p_Pressed = GameObject.FindGameObjectWithTag("Left").GetComponent<SteamVR_TrackedController>().pressed;
        }

        if (_InputGachetteL)
        {
            p_TouchedColor = false;
            Color col = Color.white;
            _light.color = col;
        }

        if (isTarget && p_TouchedColor)
        {
            p_ColorVector = GameObject.FindGameObjectWithTag("Left").GetComponent<SteamVR_TrackedController>().color;


            if (p_ColorVector.x >= 0f && p_ColorVector.x <= 0.5f && p_ColorVector.y >= 0.5f && p_ColorVector.y <= 1f)
            {
                Color col = new Color(10f / 255f, 207f / 255f, 255f / 255f, 189f / 255f);
                _light.color = col;
            
            }

            if (p_ColorVector.x >= 0.5f && p_ColorVector.x <= 1f && p_ColorVector.y >= 0f && p_ColorVector.y <= 0.5f)
            {
                Color col = new Color(222f / 255f, 67f / 255f, 150f / 255f, 202f / 255f);
                _light.color = col;
           
            }

            if (p_ColorVector.x >= 0.5f && p_ColorVector.x <= 1f && p_ColorVector.y >= -0.5f && p_ColorVector.y <= 0f)
            {

                Color col = new Color(2f / 255f, 5f / 255f, 62f / 255f, 255f / 255f);
                _light.color = col;
      
            }

            if (p_ColorVector.x >= 0f && p_ColorVector.x <= 0.5f && p_ColorVector.y >= -1f && p_ColorVector.y <= -0.5f)
            {
                Color col = new Color(248f / 255f, 255f / 255f, 33f / 255f, 161f / 255f);
                _light.color = col;
      
            }

            if (p_ColorVector.x >= -0.5f && p_ColorVector.x <= 0f && p_ColorVector.y >= -1f && p_ColorVector.y <= 0.5f)
            {

                Color col = Color.red;
                _light.color = col;
   
            }

            if (p_ColorVector.x >= -1f && p_ColorVector.x <= -0.5f && p_ColorVector.y >= -1f && p_ColorVector.y <= -0.5f)
            {

                Color col = new Color(8f / 255f, 85f / 255f, 28f / 255f, 198f / 255f);
                _light.color = col;

            }

            if (p_ColorVector.x >= -1f && p_ColorVector.x <= -0.5f && p_ColorVector.y >= 0f && p_ColorVector.y <= 0.5f)
            {

                Color col = new Color(101f / 255f, 6f / 255f, 79f / 255f, 200f / 255f);
                _light.color = col;

            }

            if (p_ColorVector.x >= -0.5f && p_ColorVector.x <= 0f && p_ColorVector.y >= 0.5f && p_ColorVector.y <= 1f)
            {             
                Color col = new Color(255f / 255f, 129f / 255f, 20f / 255f, 159f / 255f);
                _light.color = col;
 
            }

        }
    }
}
