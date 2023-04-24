using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMODUnity;
using UnityEngine;

public class GazerController : MonoBehaviour {

    [SerializeField] private List<PanelLightComponent> allLights;
    [Space]
    [SerializeField] private StudioEventEmitter sfxOn;
    [SerializeField] private StudioEventEmitter sfxOff;

    public void OnEnable() {
        StartCoroutine(UpdateAmbientRoutine());
    }

    public IEnumerator UpdateAmbientRoutine() {
        yield return CoUtils.Wait(.1f);
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

    public IEnumerator BootRoutine(bool on) {
        if (on) {
            sfxOn.Play();
        } else {
            sfxOff.Play();
        }
        float duration = 2f;
        var randomLights = allLights.OrderBy(a => Guid.NewGuid()).ToList();
        foreach (var light in randomLights) {
            light.PlayBootupSFX();
            light.IsEvil = Global.Instance.Data.Time == TimeblockType.Midnight;
            light.IsShutDown = !on;

            yield return CoUtils.Wait(duration / randomLights.Count());
        }
    }

    private void BootGood(int index, bool evil = false) {
        var light = allLights[index];
        light.IsEvil = evil;
        light.IsShutDown = false;
    }
}