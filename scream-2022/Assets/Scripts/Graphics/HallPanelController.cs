using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(PanelLightComponent))]
public class HallPanelController : MonoBehaviour {

    private new PanelLightComponent light;
    public PanelLightComponent Light => light ?? (light = GetComponent<PanelLightComponent>());

    public void Start() {
        StartCoroutine(StartRoutine());
    }

    public IEnumerator StartRoutine() {
        switch (Global.Instance.Data.Time) {
            case TimeblockType.Afternoon:
            case TimeblockType.Evening:
                Light.IsEvil = false;
                Light.preferRunning = true;
                Light.IsShutDown = false;
                break;
            case TimeblockType.Midnight:
                yield return CoUtils.Wait(.5f);
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