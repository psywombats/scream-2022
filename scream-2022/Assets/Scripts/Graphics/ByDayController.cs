using System.Collections.Generic;
using UnityEngine;

public class ByDayController : MonoBehaviour {

    [SerializeField] private List<GameObject> morningObjs;
    [SerializeField] private List<GameObject> afternoonObjs;
    [SerializeField] private List<GameObject> eveningObjs;
    [SerializeField] private List<GameObject> midnightObjs;
    [Space]
    [SerializeField] private List<GameObject> dayObjs;
    [SerializeField] private List<GameObject> nightObjs;

    public void Start() {
        UpdateObjs();
    }

    public void Update() {
        UpdateObjs(); //dumb
    }

    private void UpdateObjs() {
        var time = Global.Instance.Data.Time;
        foreach (var obj in morningObjs) {
            obj.SetActive(time == TimeblockType.Morning);
        }
        foreach (var obj in afternoonObjs) {
            obj.SetActive(time == TimeblockType.Afternoon);
        }
        foreach (var obj in eveningObjs) {
            obj.SetActive(time == TimeblockType.Evening);
        }
        foreach (var obj in midnightObjs) {
            obj.SetActive(time == TimeblockType.Midnight);
        }
        foreach (var obj in dayObjs) {
            obj.SetActive(time == TimeblockType.Morning || time == TimeblockType.Afternoon);
        }
        foreach (var obj in nightObjs) {
            obj.SetActive(time == TimeblockType.Evening || time == TimeblockType.Midnight);
        }
    }
}
