using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Doors : MonoBehaviour
{
    public KeyCode interact = KeyCode.F;
    private float angle = 0;
    private bool opening = false;
    private bool open = false;
    public float openSpeed = 500;
    private bool inRange = false;
    Quaternion startAngle;
    public Vector3 Axis = Vector3.up;
    Quaternion targetRotation;

    public Key key;
    public bool permanentlyLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        startAngle = gameObject.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interact) && inRange)
        {
            if (TryOpen())
            {
                open = !open;
                targetRotation = startAngle * Quaternion.AngleAxis(open ? 0 : -90, Axis);
            }
        }

        if (opening)
        {
            var speed = openSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, speed);

            angle = Quaternion.Angle(transform.localRotation, targetRotation);
            if (angle == 0) { opening = false; }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    public bool TryOpen()
    {
        opening = !permanentlyLocked && (key == null || key.pickedUp);
        return opening;
    }
}