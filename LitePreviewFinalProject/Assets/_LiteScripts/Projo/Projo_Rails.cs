using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projo_Rails : MonoBehaviour
{

    Light _Light;
    AudioSource _Audio;
    public float time;
    float timer;

    void Start()
    {
        _Light = GetComponent<Light>();
        _Audio = GetComponent<AudioSource>();
        _Audio.enabled = false;
        _Light.enabled = false;
    }


    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= time)
        {
            _Audio.enabled = true;
            _Light.enabled = true;
        }
    }
}
