using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraSelector : MonoBehaviour
{
    public Transform[] cameraPositions;
    public float speed = 1;
    float t = 0;
    public float maxDistance = 0.99f;

    public bool Manual = true;
    public bool selectNewParent = false;
    public UnityEvent finishedEvent;
    // Start is called before the first frame update
    void Start()
    {
        currentCamera = currentTarget;
        transform.SetPositionAndRotation(cameraPositions[currentTarget].position, cameraPositions[currentTarget].rotation);
    }

    public int currentTarget = 0;
    int currentCamera = 0;
    bool finished = true;
    float fullDistanceDiv = 1;
    Vector3 startPos = Vector3.zero;
    Quaternion startRot = Quaternion.identity;

    // Update is called once per frame
    void Update()
    {
        if (Manual && Input.GetKeyDown(KeyCode.Tab)) currentTarget++;

        if (currentTarget >= cameraPositions.Length) currentTarget = 0;

        if(currentTarget != currentCamera)
        {
            finished = false;
            var dist = Vector3.Distance(transform.position, cameraPositions[currentTarget].position);
            if(dist > 0)
            {
                fullDistanceDiv = 1 / dist;
                if(fullDistanceDiv < 1)
                {
                    currentCamera = currentTarget;
                    startPos = transform.position;
                    startRot = transform.rotation;
                    t = 0;
                }
            }
        }

        if (!finished)
        {
            Transform current = cameraPositions[currentCamera];
            t += speed * Time.deltaTime * Vector3.Distance(transform.position, current.position) * fullDistanceDiv;
            Vector3 newPos = Vector3.Lerp(startPos, current.position, t);
            Quaternion newRot = Quaternion.Lerp(startRot, current.rotation, t);

            transform.SetPositionAndRotation(newPos, newRot);
            if (t >= 1) finished = true;
            if(selectNewParent) transform.parent = current;
            if (Vector3.Distance(transform.position, current.position) <= maxDistance) finishedEvent.Invoke();
        }
    }

    public void SetCurrent(int i)
    {
        currentTarget = i;
    }

    public void NextCamera()
    {
        currentTarget++;
        //Debug.Log("Nesx");
    }
}
