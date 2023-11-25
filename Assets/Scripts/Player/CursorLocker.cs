using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLocker : MonoBehaviour
{
    public bool cursorLocked = true;
    public bool CursorLocked
    {
        get { return cursorLocked; }
        set
        {
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !value;
            cursorLocked = value;
        }
    }

    public void SetCursorLocked(int i)
    {
        CursorLocked = i == 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        CursorLocked = cursorLocked;
    }
}
