using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using UnityEngine.UI;
using Valve.VR;

public class Player : MonoBehaviour
{
    public GameObject player;

    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;
    CapsuleCollider _Collider;
    Rigidbody rb;

    Vector2 touchpad;

    private float sensitivityX = 1.5F;
    private Vector3 playerPos;


    private int activeIdx;

    Camera playerCam;

    public AProjo lightTarget;
    public AModel modelTarget;
    public Vector3 rayPosition;
    Text _Text;
	GameObject touchHelpL, touchHelpR;

    GameObject _leftHand;

	public bool isActive = false;
    bool isSelected = false;
    public int textIndex = 0;

    public VRControllerState_t controllerState;
    [SerializeField]
    public String[] texts;


    void Start()
    {
		touchHelpL = GameObject.Find("TouchHelp_L");
        touchHelpR = GameObject.Find("TouchHelp_R");
        _Text = GameObject.Find("TutoText").GetComponent<Text>();
        //playerCam = GetComponentInChildren<Camera>();
        _Collider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        controller = transform.GetChild(1).gameObject.GetComponent<SteamVR_TrackedObject>();
    }

    void LateStart()
    {

    }

    void Update()
    {
        _Collider.center = new Vector3(transform.GetChild(2).localPosition.x, 0, transform.GetChild(2).localPosition.z);
        rb.velocity = Vector3.zero;

        device = SteamVR_Controller.Input((int)controller.index);
        //If finger is on touchpad
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            //Read the touchpad values
            touchpad = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);


            // Handle movement via touchpad
            //if (touchpad.y > 0.2f || touchpad.y < -0.2f)
            {
                // Move Forward
                player.transform.position += player.transform.GetChild(2).forward * Time.deltaTime * (touchpad.y * 5f);
                player.transform.position += player.transform.GetChild(2).right * Time.deltaTime * (touchpad.x * 5f);
                player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            }
        }

        if (isActive)
        {
            touchHelpL.SetActive(false);
            touchHelpR.SetActive(false);
        }
        //rayTarget(Ray());
        ChangeText();
    }

    void ChangeText()
    {
        texts[0] = "Première Technique:\n\n La lumière en 3 points";
        texts[1] = "Cette technique sert à éclairer un personnage \nen vue de le préparer au tournage";
        texts[2] = "Premier point:\n\n La Key Light";
        texts[3] = "Elle représente la source de lumière principale.\n Sa lumière sera forte et placée en hauteur par rapport au modèle";
        texts[4] = "Nous utilisons ici le projecteur Mandarine, ceux de couleur rouge, pour représenter la Keylight";
        texts[5] = "Utilise la gâchette droite pour viser le pied du Projo et le déplacer sur la cible au sol";
        texts[6] = "Utilise la gâchette gauche sur le pied du projecteur pour régler sa taille";
        texts[7] = "Utilise la gâchette droite sur le Spot du Projo pour l'orienter sur le modèle";
        texts[8] = "Utilise la gâchette gauche sur le Spot du Projo pour régler l'intensité de la lumière";
        texts[9] = "Utilise la gâchette droite sur les rabats pour régler le volume de lumière";
        texts[10] = "Deuxième point:\n\n La Fill Light";
        texts[11] = "Elle sert à compenser les fortes ombres créées par la Key Light, sa lumière est deux fois moins forte et sa hauteur plus basse";
        texts[12] = "Utilise un deuxième Projo Mandarine pour l'installer à l'endroit indiqué et le régler";
        texts[13] = "Troisième point:\n\n La Back Light";
        texts[14] = "Son rôle est d'éclairer le dos du modèle pour le séparer du décor, plusieurs configurations sont possibles";
        texts[15] = "Restons sur une configuration classique et réglons-la comme la Key Light";
        texts[16] = "Bien joué, tu viens de réaliser ta lumière en 3 points.";
        texts[17] = "Pour installer la scène que nous allons préparer, appuie avec la gâchette de la manette droite en visant l'interrupteur rouge.";
        texts[18] = "De la même manière, tu peux allumer la télévision pour avoir le rendu de ta camera en temps réel.";
        texts[19] = "Maintenant que tu sais faire, tu peux préparer la lumière en 3 points pour le modèle.";

		if (isActive)
        	_Text.text = texts[textIndex];
    }
}
    /*
    void rayTarget(object rayTarget)
    {
        if (rayTarget != null && (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)))
        {
            isSelected = true;
        }

        if (isSelected && (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1)))
        {
            isSelected = false;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            Debug.Log("5f");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12)
        {
            Debug.Log("4f");
        }
    }

    object Ray()
    {
        /*
        RaycastHit hit;
        RaycastHit[] hits;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

        hits = Physics.RaycastAll(ray);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.tag == "Walls")
            {
                rayPosition = hits[i].point;
            }
        }


        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Malette")
            {
                verticalSpeed = Input.GetAxis("Mouse Y");

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

        return null;
    }
}


/*
   if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            rayTarget.GetComponentInParent<AProjo>().isTarget = false;
            fpsController.enabled = true;
        }

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (rayTarget != null)
                {
                    if (hit.collider.gameObject != rayTarget)
                    {
                        rayTarget.GetComponent<AProjo>().isTarget = false;
                    }
                }

                if (hit.collider.gameObject.layer == 12)
                {
                    rayTarget = hit.collider.gameObject;
                    rayTarget.GetComponentInParent<AProjo>().isTarget = true;
                    fpsController.enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }*/
