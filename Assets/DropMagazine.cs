using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMagazine : MonoBehaviour
{
    public Transform from;
    public GameObject magazine;
    public void Drop()
    {
        GameObject m = Instantiate(magazine);
        Destroy(m, 120);
        m.transform.SetPositionAndRotation(from.position, from.rotation);
    }
}
