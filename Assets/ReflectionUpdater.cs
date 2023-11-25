using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionUpdater : MonoBehaviour
{
    ReflectionProbe probe;
    // Start is called before the first frame update
    void Start()
    {
        probe = GetComponent<ReflectionProbe>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) probe.RenderProbe();
    }
}
