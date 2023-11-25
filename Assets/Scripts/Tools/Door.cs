using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Key key;
    public bool permanentlyLocked = false;

    public bool TryOpen()
    {
        bool s = !permanentlyLocked && (key == null || key.pickedUp);
        return s;
    }
}
