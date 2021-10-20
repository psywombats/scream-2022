using System.Collections.Generic;
using UnityEngine;

public class SwitchControl : MonoBehaviour {

    [SerializeField] private string switchName = null;
    [SerializeField] private List<GameObject> toEnable = null;
    [SerializeField] private List<GameObject> toDisable = null;

    protected void Start() {
        var enable = Global.Instance.Data.GetSwitch(switchName);
        foreach (var obj in toEnable) {
            obj.SetActive(enable);
        }
        foreach (var obj in toDisable) {
            obj.SetActive(!enable);
        }
    }
}