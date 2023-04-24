using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class VideoManager : SingletonBehavior {

    public static VideoManager Instance => Global.Instance.Video;

    private VideoPlayer player;

    public void Start() {
        player = gameObject.AddComponent<VideoPlayer>();
        player.targetTexture = Resources.Load<RenderTexture>("Textures/RenderTextures/VideoBank");

#if UNITY_WEBGL
        player.url = "https://psyvideo.github.io/page/master2.webm";
        //player.url = "https://theroccob.github.io/myvideo/video.mp4";
        player.source = VideoSource.Url;
        player.playOnAwake = false;
#else
        player.clip = Resources.Load<VideoClip>("Video/master");
        player.source = VideoSource.VideoClip;
        player.Play();
        player.isLooping = true;
#endif
    }

    public void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            player.Play();
            player.isLooping = true;
        }
    }

    public RunningVideo RequestVideo(VideoData.Type type) {
        var index = Random.Range(0, type == VideoData.Type.Evil ? 8 : 7);
        var data = IndexDatabase.Instance.Videos.GetData((type == VideoData.Type.Evil ? "Spooky" : "Calm") + "0" + index);
        var uvs = new Vector2(
            (index % 4) * .25f,
            (1 - (index / 4)) * .25f);
        if (type == VideoData.Type.Evil) {
            uvs = new Vector2(uvs.x, uvs.y + .5f);
        }
        return new RunningVideo() {
            data = data,
            uvs = uvs,
        };
    }

    public class RunningVideo {
        public VideoData data;
        public Vector2 uvs;
    }
}
