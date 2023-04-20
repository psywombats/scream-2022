using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PanelLightComponent : MonoBehaviour {

    [SerializeField] public SpriteRenderer checker = null;

    private PanelLightManager manager;

    public void Start() {
        manager = FindObjectOfType<PanelLightManager>();
        manager.allLights.Add(this);
        //manager.CheckLighting();
    }

    public void OnDestroy() {
        manager.allLights.Remove(this);
        //manager.CheckLighting();
    }
}