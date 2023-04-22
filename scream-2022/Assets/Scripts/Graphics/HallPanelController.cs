using UnityEngine;

[RequireComponent(typeof(PanelLightComponent))]
public class HallPanelController : MonoBehaviour {

    private new PanelLightComponent light;
    public PanelLightComponent Light => light ?? (light = GetComponent<PanelLightComponent>());

    public void Start() {
        switch (Global.Instance.Data.Time) {
            case TimeblockType.Afternoon:
            case TimeblockType.Evening:
                Light.IsEvil = false;
                Light.preferRunning = true;
                Light.IsShutDown = false;
                break;
            default:
                Light.IsShutDown = true;
                break;
        }
    }
}