using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform head;
    public float sway = 0.2f;
    public float velocity = 1;
    public float landingDelay = 0.2f;
    public float landingTime = 0;
    public float swayAmp = 0.1f;
    public float swayMult = 1;
    public float t = 0;
    public float bonusMovement = 0;
    // Start is called before the first frame update

    private void Start()
    {
        landingTime = landingDelay;
    }
    public void Shake(float speed, bool aired, bool pAired)
    {
        speed = Mathf.Max(Time.deltaTime * 0.5f, speed);
        if (!aired && !(landingTime < landingDelay))
        {
            if(velocity != 1)
            {
                velocity += Time.deltaTime;
                if (velocity > 1) velocity = 1;
            }
            if(swayMult > 1)
            {
                swayMult -= Time.deltaTime;
                if (swayMult < 1) swayMult = 1;
            }
        }
        else
        {
            if (pAired || landingTime < landingDelay)
            {
                if (velocity != 1)
                {
                    velocity += Time.deltaTime;
                    if (velocity > 1) velocity = 1;
                }
                if (landingTime == landingDelay) landingTime = 0;
                float delayProc = landingTime / landingDelay;
                landingTime += Time.deltaTime;
                swayMult += Time.deltaTime * 0.5f;
                if (swayMult > 1.6f) swayMult = 1.6f;
                speed += Time.deltaTime * 20 * (1 - delayProc);
                if (landingTime > landingDelay) landingTime = landingDelay;
                bonusMovement = Mathf.Lerp(sway * 0.5f, 0, delayProc);
            }
            else
            {
                velocity -= Time.deltaTime;
                if (velocity < 0.1f) velocity = 0.1f;
                bonusMovement += Time.deltaTime;
                if (bonusMovement > sway * 0.5f) bonusMovement = sway * 0.5f;
            }
        }

        t += speed * velocity * Random.Range(0.9f,1.1f);
        if (swayAmp * t > Mathf.PI * 2) t = 0;
        float swayY = Mathf.Sin(swayAmp * t) * sway * swayMult;
        transform.localPosition = new Vector3(0, swayY + bonusMovement, 0);
    }
}