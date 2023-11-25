using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPosition : MonoBehaviour
{
    public Transform target;

    public void Switch()
    {
        transform.position = target.position;
    }
}
