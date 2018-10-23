//======= Copyright (c) Valve Corporation, All rights reserved. ===============
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public struct PointerEventArgs
{
    public uint controllerIndex;
    public uint flags;
    public float distance;
    public Transform target;
}

public delegate void PointerEventHandler(object sender, PointerEventArgs e);


public class SteamVR_LaserPointer : MonoBehaviour
{
    public bool active = true;
    public Color color;
    public float thickness = 0.002f;
    public GameObject holder;
    public GameObject pointer;
    bool isActive = false;
    public bool addRigidBody = false;
    public Transform reference;
    public event PointerEventHandler PointerIn;
    public event PointerEventHandler PointerOut;

    Transform previousContact = null;


    //
    public AProjo lightTarget;
    public AModel modelTarget;
    object _target;
    bool isSelected = false;
    public bool triggerPressed = false;
    public bool gachetteR = false;
    public bool gachetteL = false;
    bool startCheck, creditCheck;
    public Vector3 rayPosition;
    public bool touchPad = false;
    public float touchpadX;
    Player player;
    GameObject startText, creditText;
    //

    // Use this for initialization
    void Start()
    {
        startText = GameObject.Find("StartText");
        creditText = GameObject.Find("CreditsText");
        player = transform.parent.GetComponent<Player>();
        holder = new GameObject();
        holder.transform.parent = this.transform;
        holder.transform.localPosition = Vector3.zero;
        holder.transform.localRotation = Quaternion.identity;

        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.transform.parent = holder.transform;
        pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
        pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
        pointer.transform.localRotation = Quaternion.identity;
        pointer.tag = "Ray";
        BoxCollider collider = pointer.GetComponent<BoxCollider>();
        if (addRigidBody)
        {
            if (collider)
            {
                collider.isTrigger = true;
            }
            Rigidbody rigidBody = pointer.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;
        }
        else
        {
            if (collider)
            {
                Object.Destroy(collider);
            }
        }
        Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        newMaterial.SetColor("_Color", color);
        pointer.GetComponent<MeshRenderer>().material = newMaterial;
    }

    public virtual void OnPointerIn(PointerEventArgs e)
    {
        if (PointerIn != null)
            PointerIn(this, e);
    }

    public virtual void OnPointerOut(PointerEventArgs e)
    {
        if (PointerOut != null)
            PointerOut(this, e);
    }

    void rayTarget(object rayTarget)
    {
        if (rayTarget != null && (gachetteR || gachetteL))//&& (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)))
        {
            if (gameObject.tag == "Left")
            {
                isSelected = true;
                if (lightTarget != null)
                    lightTarget._InputGachetteL = true;
                if (modelTarget != null)
                    modelTarget._InputGachetteL = true;
            }
            if (gameObject.tag == "Right")
            {
                isSelected = true;
                if (lightTarget != null)
                    lightTarget._InputGachetteR = true;
                if (modelTarget != null)
                    modelTarget._InputGachetteR = true;
            }
        }

        if (isSelected && !gachetteR && !gachetteL)//&& (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1)))
        {
            if (gameObject.tag == "Left")
            {
                isSelected = false;
                if (lightTarget != null)
                    lightTarget._InputGachetteL = false;
                if (modelTarget != null)
                    modelTarget._InputGachetteL = false;
            }
            if (gameObject.tag == "Right")
            {
                isSelected = false;
                if (lightTarget != null)
                    lightTarget._InputGachetteR = false;
                if (modelTarget != null)
                    modelTarget._InputGachetteR = false;
            }
        }

        if (!isSelected && rayTarget == null)
        {
            if (lightTarget != null)
            {
                lightTarget.isTarget = false;
                lightTarget = null;
            }
            else if (modelTarget != null)
            {
                modelTarget.isTarget = false;
                modelTarget = null;
            }
        }

        if (isSelected && rayTarget != null)
        {
            if (lightTarget != null && modelTarget != null)
            {
                if (rayTarget.GetType() == lightTarget.GetType())
                {
                    lightTarget.isTarget = false;
                    lightTarget = null;
                }
            }
            if (modelTarget != null && lightTarget != null)
            {
                if (rayTarget.GetType() == modelTarget.GetType())
                {
                    modelTarget.isTarget = false;
                    modelTarget = null;
                }
            }
        }
    }

    object Ray()
    {

        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        RaycastHit[] hits;
        bool bHit = Physics.Raycast(raycast, out hit);

        hits = Physics.RaycastAll(raycast);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.tag == "Walls")
            {
                rayPosition = hits[i].point;
            }
        }


        if (Physics.Raycast(raycast, out hit))
        {
            if (hit.collider.tag == "Text" && touchPad == true)
            {
                if (!player.isActive)
                {
                    player.isActive = true;
                }
                else
                {
                    if (player.textIndex > 0 && touchpadX < 0)
                        player.textIndex -= 1;

                    if (player.textIndex < 19 && touchpadX > 0)
                        player.textIndex += 1;
                }
                touchPad = false;
            }

            if (startText != null)
            {
                if (hit.collider.tag == "Start")
                {
                    startCheck = true;
                    startText.GetComponent<Text>().color = Color.red;
                }

                if (hit.collider.tag == "Start" && gachetteR == true)
                {
                    SceneManager.LoadScene(1);
                }
            }

            if (creditText != null)
            {
                if (hit.collider.tag == "Credit")
                {
                    creditCheck = true;
                    creditText.GetComponent<Text>().color = Color.red;
                }

                if (hit.collider.tag == "Credit" && gachetteR == true)
                {
                    SceneManager.LoadScene(2);
                }
            }

            if (isSelected)
            {
                if (lightTarget != null)
                {
                    return lightTarget;
                }
                if (modelTarget != null)
                {
                    return modelTarget;
                }
            }

            if (lightTarget == null && modelTarget == null)
            {
                if (hit.collider.gameObject.layer == 12)
                {
                    lightTarget = hit.collider.gameObject.GetComponentInParent<AProjo>();
                    lightTarget.isTarget = true;
                    return lightTarget;
                }
                else if (hit.collider.gameObject.layer == 8)
                {
                    modelTarget = hit.collider.gameObject.GetComponentInParent<AModel>();
                    modelTarget.isTarget = true;
                    return modelTarget;
                }
            }

            if (lightTarget != null && hit.collider.gameObject.layer == 12)
            {
                if (hit.collider.gameObject != lightTarget.gameObject)
                {
                    lightTarget.isTarget = false;
                    lightTarget = hit.collider.gameObject.GetComponentInParent<AProjo>();
                    lightTarget.isTarget = true;
                    return lightTarget;
                }
                return lightTarget;
            }
            if (modelTarget != null && hit.collider.gameObject.layer == 8)
            {
                return modelTarget;
            }
        }
        else if (startCheck)
        {
            startCheck = false;
            startText.GetComponent<Text>().color = Color.white;
        }
        else if (creditCheck)
        {
            creditCheck = false;
            creditText.GetComponent<Text>().color = Color.white;
        }
        return null;
    }


    // Update is called once per frame
    void Update()
    {
        rayTarget(Ray());

        if (!isActive)
        {
            isActive = true;
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

        float dist = 100f;

        SteamVR_TrackedController controller = GetComponent<SteamVR_TrackedController>();

        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit);

        if (previousContact && previousContact != hit.transform)
        {
            PointerEventArgs args = new PointerEventArgs();
            if (controller != null)
            {
                args.controllerIndex = controller.controllerIndex;
            }
            args.distance = 0f;
            args.flags = 0;
            args.target = previousContact;
            OnPointerOut(args);
            previousContact = null;
        }
        if (bHit && previousContact != hit.transform)
        {
            PointerEventArgs argsIn = new PointerEventArgs();
            if (controller != null)
            {
                argsIn.controllerIndex = controller.controllerIndex;
            }
            argsIn.distance = hit.distance;
            argsIn.flags = 0;
            argsIn.target = hit.transform;
            OnPointerIn(argsIn);
            previousContact = hit.transform;
        }
        if (!bHit)
        {
            previousContact = null;
        }
        if (bHit && hit.distance < 100f)
        {
            dist = hit.distance;
        }

        if (controller != null && controller.triggerPressed)
        {
            pointer.transform.localScale = new Vector3(thickness * 5f, thickness * 5f, dist);
        }
        else
        {
            pointer.transform.localScale = new Vector3(thickness, thickness, dist);
        }
        pointer.transform.localPosition = new Vector3(0f, 0f, dist / 2f);
    }
}
