using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(PanelLightComponent))]
public class HallPanelController : MonoBehaviour {

    private new PanelLightComponent light;
    public PanelLightComponent Light => light ?? (light = GetComponent<PanelLightComponent>());

    public async void Start() {
        switch (Global.Instance.Data.Time) {
            case TimeblockType.Afternoon:
            case TimeblockType.Evening:
                Light.IsEvil = false;
                Light.preferRunning = true;
                Light.IsShutDown = false;
                break;
            case TimeblockType.Midnight:
                await Task.Delay(500);
                Light.IsEvil = true;
                Light.preferRunning = false;
                light.IsShutDown = false;
                break;
            default:
                Light.IsShutDown = true;
                break;
        }
    }
}