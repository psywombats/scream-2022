using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GazerController : MonoBehaviour {

    [SerializeField] private List<PanelLightComponent> allLights;

    public void OnEnable() {
        UpdateAmbient();
    }

    public async void UpdateAmbient() {
        await Task.Delay(100);
        foreach (var light in allLights) {
            light.IsShutDown = true;
        }
        switch (Global.Instance.Data.Time) {
            case TimeblockType.Morning:
                break;
            case TimeblockType.Afternoon:
                BootGood(2);
                BootGood(9);
                break;
            case TimeblockType.Evening:
                if (!Global.Instance.Data.GetSwitch("pt2_08")) {
                    for (var i = 0; i < allLights.Count; i += 1) {
                        if (i == 3 || i == 8) {
                            continue;
                        }
                        BootGood(i, i == 11);
                    }
                }
                break;
            case TimeblockType.Midnight:
                break;
        }
    }

    public void DisableAmbient() {
        foreach (var light in allLights) {
            light.IsShutDown = true;
        }
    }

    public async Task BootAsync(bool on) {
        float duration = 2f;
        var randomLights = allLights.OrderBy(a => Guid.NewGuid()).ToList();
        foreach (var light in randomLights) {
            light.PlayBootupSFX();
            light.IsEvil = Global.Instance.Data.Time == TimeblockType.Midnight;
            light.IsShutDown = !on;

            await Task.Delay((int)(1000 * duration / randomLights.Count()));
        }
    }

    private void BootGood(int index, bool evil = false) {
        var light = allLights[index];
        light.IsEvil = evil;
        light.IsShutDown = false;
    }
}