using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GazerController : MonoBehaviour {

    [SerializeField] private List<PanelLightComponent> allLights;

    public async Task BootAsync(bool on) {
        float duration = 2f;
        var randomLights = allLights.OrderBy(a => Guid.NewGuid()).ToList();
        foreach (var light in randomLights) {
            light.PlayBootupSFX();
            light.IsEvil = false;
            light.IsShutDown = !on;

            await Task.Delay((int)(1000 * duration / randomLights.Count()));
        }
    }
}