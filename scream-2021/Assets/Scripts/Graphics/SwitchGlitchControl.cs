using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DepthCamComponent))]
public class SwitchGlitchControl : MonoBehaviour {

    [SerializeField] private List<string> switchNames = null;

    private DepthCamComponent cam;
    private DepthCamComponent Cam => cam ?? (cam = GetComponent<DepthCamComponent>());

    public void Update() {
        var use = true;
        foreach (var switchName in switchNames) {
            if (!Global.Instance.Data.GetSwitch(switchName)) {
                use = false;
                break;
            }
        }
        Cam.GlitchFromSwitch = use;
    }

    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, Cam.rangeMin);
        Gizmos.DrawWireSphere(transform.position, Cam.rangeMax);
    }
}