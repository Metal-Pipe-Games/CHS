using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndOfSpash : MonoBehaviour
{
    VideoPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        player.loopPointReached += Player_loopPointReached;
    }

    private void Player_loopPointReached(VideoPlayer source)
    {
        SceneTransporter.GoToScene(2);
    }
}
