using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light Light;
    public bool activated = true;
    public float interval = 2;
    public float duration = 0.1f;
    public bool isOn = false;
    public bool waiting = false;

    public float maxStrength = 10000;
    public float minStrength = 0;

    float t1 = 0;
    float t2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        Light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && Light != null)
        {
            if (!waiting)
            {
                float strength;
                t2 += Time.deltaTime/duration;
                switch (isOn)
                {
                    case false:
                        strength = Mathf.Lerp(minStrength, maxStrength, t2);
                        break;
                    case true:
                        strength = Mathf.Lerp(maxStrength, minStrength, t2);
                        break;
                }
                Light.intensity = strength;
                if(t2 >= duration)
                {
                    waiting = true;
                    isOn = !isOn;
                    t2 = 0;
                }
            }
            else
            {
                t1 += Time.deltaTime * (isOn ? 2:1);
                if(t1 >= interval)
                {
                    t1 = 0;
                    waiting = false;
                }
            }
        }
    }
}
