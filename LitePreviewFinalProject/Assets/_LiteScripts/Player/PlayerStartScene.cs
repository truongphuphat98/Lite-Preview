using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStartScene : MonoBehaviour
{
    public KeyCode startCode = KeyCode.Mouse0;
    Camera playerCam;
    GameObject startText;

    void Start()
    {
        playerCam = GetComponentInChildren<Camera>();
        startText = GameObject.Find("StartText");
    }


    void Update()
    {
        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Start")
            {
                if (Input.GetKeyDown(startCode))
                {
                    SceneManager.LoadScene(1);
                }
                hit.collider.gameObject.GetComponent<Outline>().effectColor = Color.red;
            }
            else
            {
                startText.GetComponent<Outline>().effectColor = Color.black;
            }
        }
        else
        {
            startText.GetComponent<Outline>().effectColor = Color.black;
        }
    }
}
