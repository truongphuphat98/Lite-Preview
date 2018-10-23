using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeModel : MonoBehaviour
{
    Transform _Transform;
    GameObject model;
    float distance;

    void Start()
    {
        _Transform = transform;
        model = GameObject.Find("Model");
        model.SetActive(false);
    }


    void Update()
    {
        if (_Transform.position.y < -3f)
        {
            model.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
