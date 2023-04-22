using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : SingletonBehavior {

    public static VideoManager Instance => Global.Instance.Video;

    private const int MaxVideosPerBank = 6;

    private Dictionary<VideoData.Type, List<ClipData>> clipsByType;
    private Dictionary<VideoData.Type, List<RunningVideo>> activeVids = new Dictionary<VideoData.Type, List<RunningVideo>>();

    public void Start() {
        EnsureClips();
    }

    public void Update() {
        foreach (var bank in activeVids.Values) {
            foreach (var vid in bank) {
                vid.elapsed += Time.deltaTime;
                if (vid.elapsed >= vid.graduateAt) {
                    var dataBank = clipsByType[vid.data.data.type];
                    var off = Random.Range(0, dataBank.Count - 1);
                    for (var i = 0; i < dataBank.Count; i += 1) {
                        var newData = dataBank[(i + off) % dataBank.Count];
                        if (newData.clipHandles == 0) {
                            vid.data.clipHandles -= 1;
                            vid.data = newData;
                            vid.data.clipHandles += 1;
                            vid.player.clip = newData.data.clip;
                            vid.player.isLooping = true;
                            vid.player.Play();
                            break;
                        }
                    }
                    vid.Graduate();
                }
            }
        }
    }

    public RunningVideo RequestVideo(VideoData.Type type, bool preferRunning) {
        EnsureClips();

        if (!activeVids.TryGetValue(type, out var bank))  {
            bank = new List<RunningVideo>();
            activeVids.Add(type, bank);
        }

        if (bank.Count > 0 && (preferRunning || bank.Count >= MaxVideosPerBank)) {
            var vid = bank[Random.Range(0, bank.Count - 1)];
            vid.vidHandles += 1;
            return vid;
        }

        var dataBank = clipsByType[type];
        var off = Random.Range(0, dataBank.Count - 1);
        for (var i = 0; i < dataBank.Count; i += 1) {
            var data = dataBank[(i + off) % dataBank.Count];
            if (data.clipHandles == 0) {
                var vid = CreateVideo(data);
                return vid;
            }
        }

        if (bank.Count > 0) {
            var vid = bank[Random.Range(0, bank.Count - 1)];
            vid.vidHandles += 1;
            return vid;
        }

        return null;
    }

    public void ReleaseVideo(RunningVideo vid) {
        vid.vidHandles -= 1;
        if (vid.vidHandles == 0) {
            DestroyVideo(vid);
        }
    }

    private void DestroyVideo(RunningVideo vid) {
        vid.player.Stop();
        vid.player.targetTexture = null;
        vid.data.clipHandles -= 1;
        Destroy(vid.player);
        Destroy(vid.tex);
        activeVids[vid.data.data.type].Remove(vid);
    }

    private RunningVideo CreateVideo(ClipData data) {
        var tex = new RenderTexture((int)data.data.clip.width, (int)data.data.clip.height, 0, RenderTextureFormat.ARGB32);
        var vid = new RunningVideo() {
            data = data,
            tex = tex,
            player = gameObject.AddComponent<VideoPlayer>(),
            vidHandles = 1,
        };
        data.clipHandles += 1;
        vid.player.targetTexture = vid.tex;
        vid.player.clip = data.data.clip;
        vid.player.audioOutputMode = VideoAudioOutputMode.None;
        vid.player.Play();
        vid.Graduate();
        activeVids[data.data.type].Add(vid);
        return vid;
    }

    private void EnsureClips() {
        if (clipsByType != null) {
            return;
        }
        clipsByType = new Dictionary<VideoData.Type, List<ClipData>>();
        var clips = IndexDatabase.Instance.Videos.GetAll();
        foreach (var clip in clips) {
            if (!clipsByType.TryGetValue(clip.type, out var list)) {
                list = new List<ClipData>();
                clipsByType.Add(clip.type, list);
            }
            list.Add(new ClipData() {
                data = clip,
            });
        }
    }

    private void OnDestroy() {
        foreach (var bank in activeVids.Values) {
            foreach (var clip in bank) {
                // concurrent mod, lazy
            }
        }
    }

    public class RunningVideo {
        public ClipData data;
        public RenderTexture tex;
        public VideoPlayer player;
        public float elapsed;
        public float graduateAt;
        public int vidHandles;

        public void Graduate() {
            graduateAt = Mathf.Min(Random.Range(2f, 4f), (float)data.data.clip.length);
            elapsed = 0f;
        }
    }

    public class ClipData {
        public VideoData data;
        public int clipHandles;
    }
}
