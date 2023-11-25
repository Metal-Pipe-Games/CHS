using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RenderArea : MonoBehaviour
{
    public bool invert = false;
    public bool inArea = false;

    private void Start()
    {
        inArea = invert;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inArea = !invert;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inArea = invert;
        }
    }
}
