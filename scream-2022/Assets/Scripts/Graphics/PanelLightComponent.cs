using System.Collections.Generic;
using UnityEngine;

public class PanelLightComponent : MonoBehaviour {

    [SerializeField] public SpriteRenderer checker = null;
    [SerializeField] public List<Light> lights = null;
    [SerializeField] public List<GameObject> goodModes = null;
    [SerializeField] public List<GameObject> evilModes = null;
    [Space]
    [SerializeField] public MeshRenderer goodRenderer;
    [SerializeField] public MeshRenderer badRenderer;
    [SerializeField] public Material badMaterial;
    [SerializeField] public Material goodMaterial;
    [SerializeField] public bool preferRunning;

    private bool isEvil;
    public bool IsEvil {
        get => isEvil;
        set {
            if (value != isEvil) {
                isEvil = value;
                if (vid != null) {
                    VideoManager.Instance.ReleaseVideo(vid);
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
        manager = FindObjectOfType<CorridorController>();
        if (manager != null) {
            manager.allLights.Add(this);
            IsEvil = manager.DefaultEvil;
            IsShutDown = manager.DefaultShutdown;
            IsLimited = true;
        } else {
            IsEvil = false;
            IsShutDown = true;
            IsLimited = true;
        }
    }

    public void OnDestroy() {
        if (manager != null) {
            manager.allLights.Remove(this);
        }
    }

    public void PlayBootupSFX() {
        // TODO: sfx
    }

    private void UpdateMode() {
        foreach (var item in goodModes) {
            item.SetActive(!IsShutDown && !IsEvil);
        }
        foreach (var item in evilModes) {
            item.SetActive(!IsShutDown && IsEvil);
        }
        if (IsShutDown && vid != null) {
            VideoManager.Instance.ReleaseVideo(vid);
        }
        if (!IsShutDown && vid == null && 
            ((!IsEvil && goodRenderer != null) || (IsEvil && badRenderer != null))) {
            vid = VideoManager.Instance.RequestVideo(IsEvil ? VideoData.Type.Evil : VideoData.Type.Good, preferRunning);
            if (IsEvil) {
                if (badRenderer.material != null) { Destroy(badRenderer.material); }
                badRenderer.material = new Material(badMaterial);
                badRenderer.material.mainTexture = vid.tex;
            } else {
                if (goodRenderer.material != null) { Destroy(goodRenderer.material); }
                goodRenderer.material = new Material(goodMaterial);
                goodRenderer.material.mainTexture = vid.tex;
            }
            foreach (var light in lights) {
                light.color = vid.data.data.color;
            }
        }
    }
}