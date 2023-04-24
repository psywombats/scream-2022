using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMODUnity;
using UnityEngine;

public class PanelLightComponent : MonoBehaviour {

    [SerializeField] public SpriteRenderer checker = null;
    [SerializeField] public List<Light> lights = null;
    [SerializeField] public List<GameObject> goodModes = null;
    [SerializeField] public List<GameObject> evilModes = null;
    [Space]
    [SerializeField] public MeshRenderer goodRenderer;
    [SerializeField] public MeshRenderer badRenderer;
    [SerializeField] public bool preferRunning;
    [Space]
    [SerializeField] public StudioEventEmitter sfxBoot;
    [SerializeField] public StudioEventEmitter sfxSwap;

    private float elapsed = 0f;
    private float graduates;

    private bool isEvil;
    public bool IsEvil {
        get => isEvil;
        set {
            if (value != isEvil) {
                isEvil = value;
                if (vid != null) {
                    PlaySwapSFX();
                    vid = null;
                }
            }
            UpdateMode();
        }
    }

    private bool isShutDown = true;
    public bool IsShutDown {
        get => isShutDown;
        set {
            isShutDown = value;
            UpdateMode();
        }
    }

    private bool isLimited;
    public bool IsLimited {
        get => isLimited;
        set {
            if (isLimited != value) {
                isLimited = value;
                foreach (var light in lights) {
                    light.enabled = !isLimited;
                }
            }
        }
    }

    private VideoManager.RunningVideo vid;
    private CorridorController manager;

    public void Start() {
        StartCoroutine(StartRoutine());
    }
    private IEnumerator StartRoutine() {
        yield return null;
        manager = FindObjectOfType<CorridorController>();
        if (manager != null) {
            manager.allLights.Add(this);
            IsEvil = manager.DefaultEvil;
            IsShutDown = manager.DefaultShutdown;
            IsLimited = true;
        } else {
            IsLimited = false;
        }
    }

    public void Update() {
        if (elapsed > graduates || graduates == 0) {
            graduates = Random.Range(4f, 12f);
            if (elapsed > 0) {
                elapsed = 0f;
                vid = null;
                UpdateMode();
            }
        }
        if (!IsShutDown) {
            elapsed += Time.deltaTime;
        }
    }

    public void OnDestroy() {
        if (manager != null) {
            manager.allLights.Remove(this);
        }
    }

    public void PlayBootupSFX() {
        sfxBoot.Play();
    }

    public void PlaySwapSFX() {
        sfxSwap.Play();
    }

    private void UpdateMode() {
        foreach (var item in goodModes) {
            item.SetActive(!IsShutDown && !IsEvil);
        }
        foreach (var item in evilModes) {
            item.SetActive(!IsShutDown && IsEvil);
        }
        if (!IsShutDown && vid == null && 
            ((!IsEvil && goodRenderer != null) || (IsEvil && badRenderer != null))) {
            vid = VideoManager.Instance.RequestVideo(IsEvil ? VideoData.Type.Evil : VideoData.Type.Good);
            var renderer = IsEvil ? badRenderer : goodRenderer;
            renderer.GetComponent<MeshFilter>().mesh.SetUVs(0, new Vector2[] {
                    new Vector2(vid.uvs.x + .00f, vid.uvs.y + .00f),
                    new Vector2(vid.uvs.x + .25f, vid.uvs.y + .00f),
                    new Vector2(vid.uvs.x + .00f, vid.uvs.y + .25f),
                    new Vector2(vid.uvs.x + .25f, vid.uvs.y + .25f),
                });
            foreach (var light in lights) {
                light.color = vid.data.color;
            }
        }
    }
}