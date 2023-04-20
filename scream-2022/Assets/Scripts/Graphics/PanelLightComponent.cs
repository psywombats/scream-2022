using System.Collections.Generic;
using UnityEngine;

public class PanelLightComponent : MonoBehaviour {

    [SerializeField] public SpriteRenderer checker = null;
    [SerializeField] public List<Light> lights = null;
    [SerializeField] public List<GameObject> goodModes = null;
    [SerializeField] public List<GameObject> evilModes = null;

    private bool isEvil;
    public bool IsEvil {
        get => isEvil;
        set {
            if (value != isEvil) {
                isEvil = value;
            }
            UpdateMode();
        }
    }

    private bool isShutDown;
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

    private CorridorController manager;

    public void Start() {
        manager = FindObjectOfType<CorridorController>();
        manager.allLights.Add(this);
        IsEvil = manager.DefaultEvil;
        IsShutDown = manager.DefaultShutdown;
        IsLimited = true;
    }

    public void OnDestroy() {
        manager.allLights.Remove(this);
    }

    private void UpdateMode() {
        foreach (var item in goodModes) {
            item.SetActive(!IsShutDown && !IsEvil);
        }
        foreach (var item in evilModes) {
            item.SetActive(!IsShutDown && IsEvil);
        }
    }
}