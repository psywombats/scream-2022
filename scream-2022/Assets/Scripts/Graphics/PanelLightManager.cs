using System.Collections.Generic;
using UnityEngine;

public class PanelLightManager : MonoBehaviour, IComparer<PanelLightComponent> {

    public List<PanelLightComponent> allLights = new List<PanelLightComponent>();

    public void Update() {
        CheckLighting();
    }

    public void CheckLighting() {
        allLights.Sort(this);
        var count = 0;

        if (allLights.Count < 8) {
            foreach (var light in allLights) {
                light.gameObject.SetActive(true);
            }
        } else {
            foreach (var light in allLights) {
                if (light.checker.isVisible) {
                    light.gameObject.SetActive(count < 8);
                    count += 1;
                } else {
                    light.gameObject.SetActive(false);
                }
            }
        }
    }

    public int Compare(PanelLightComponent x, PanelLightComponent y) {
        var avatar = AvatarEvent.Instance;
        var a = Vector3.Distance(x.transform.position, avatar.transform.position);
        var b = Vector3.Distance(y.transform.position, avatar.transform.position);
        return Mathf.RoundToInt(1000 * (a - b));
    }
}
