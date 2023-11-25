using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveSceneTrigger : MonoBehaviour
{
    public int GoToSceneId = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneTransporter.GoToScene(GoToSceneId);
        }
    }
}
