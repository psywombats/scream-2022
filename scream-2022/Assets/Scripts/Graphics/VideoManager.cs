using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class VideoManager : SingletonBehavior {

    public static VideoManager Instance => Global.Instance.Video;

    private VideoPlayer player;

    public void Start() {
        player = gameObject.AddComponent<VideoPlayer>();
        player.targetTexture = Resources.Load<RenderTexture>("Textures/RenderTextures/VideoBank");
        player.audioOutputMode = VideoAudioOutputMode.None;
#if UNITY_WEBGL
        player.clip = Resources.Load<VideoClip>("Video/master");
        player.source = VideoSource.VideoClip;
        player.Play();
#else
        player.url = "https://psywombats.github.io/video/master.mp4";
        player.source = VideoSource.Url;
        player.playOnAwake = false;
#endif

        player.isLooping = true;
        
    }

    public void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            player.Play();
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
