using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoUnload : MonoBehaviour
{
    public Camera Camera;
    public float minimulAngle = 75;
    public List<RenderPair> renderPairs = new();

    [Serializable]
    public class RenderPair
    {
        public GameObject target;
        public List<RenderArea> areas;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var pair in renderPairs)
        {
            bool isInArea = pair.areas.Count == 0 || pair.areas.Exists(v => v.inArea);

            if (isInArea)
            {
                for(int i = 0; i < pair.target.transform.childCount; i++)
                {
                    Transform child = pair.target.transform.GetChild(i);
                    Vector3 dir = (child.position - Camera.transform.position);
                    Vector3 f = Camera.transform.forward;
                    float angle = Vector2.Angle(new Vector2(f.x,f.z), new Vector2(dir.x,dir.z));

                    child.gameObject.SetActive(angle < minimulAngle);
                }

            }
            pair.target.SetActive(isInArea);
        }
    }
}