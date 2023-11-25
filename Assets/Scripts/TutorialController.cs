using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialController : MonoBehaviour
{
    public CameraSelector selector;
    public PlayableDirector director;
    public PlayableAsset pAsset;
    public Animator animator;
    public CursorLocker Locker;
    bool newGameStarted = false;
    public CanvasGroup target;
    public RectTransform targetTransform;
    public CameraFade fade;
    RectTransform Transform;
    public float speed = 1;
    float speedMult;
    bool finished = false;
    // Start is called before the first frame update
    void Start()
    {
        Transform = GetComponent<RectTransform>();

        speedMult = 1 / Vector3.Distance(Transform.position, targetTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (newGameStarted && !finished)
        {
            float t = 0;
            t += speed * Time.deltaTime * Vector3.Distance(transform.position, targetTransform.position) * speedMult;
            Vector3 newPos = Vector3.Lerp(Transform.position, targetTransform.position, t);
            target.alpha = 1 - t;

            transform.position = newPos;
            if (t >= 1) finished = true;
        }
    }

    public void NewGame()
    {
        if (!newGameStarted)
        {
            newGameStarted = true;
            Locker.CursorLocked = true;
            director.Play();
            target.interactable = false;
        }
    }

    public void LoadGame()
    {
        if (!newGameStarted)
        {
            newGameStarted = true;
            Locker.cursorLocked = true;
            director.Play(pAsset);
            target.interactable = false;
            fade.mode = FadeMode.fadeOut;
            Invoke(nameof(CutsceneLoadEnd), 5);
        }
    }

    void CutsceneLoadEnd()
    {
        SceneTransporter.GoToScene(3);
    }

    public void CameraMoveFinished()
    {
        
    }

    public void Quit()
    {
        if (!newGameStarted) Application.Quit();
    }
}
