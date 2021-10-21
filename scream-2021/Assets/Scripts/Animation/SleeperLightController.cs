using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SleeperLightController : MonoBehaviour {

    [SerializeField] private List<Light> lights = null;
    [Space]
    [SerializeField] private float shakeDuration = .2f;
    [SerializeField] private float pauseDuration = .6f;
    [SerializeField] private float endIntensity = 3f;
    [SerializeField] private float intensityDuration = 1f;
    [SerializeField] private int flickerCount = 5;
    [SerializeField] private float flickerDuration = .1f;
    [SerializeField] private float midWait = .2f;
    [SerializeField] private float cooldownDuration = .5f;

    public IEnumerator ShowRoutine() {
        var toRun = new List<Task>();
        foreach (var light in lights) {
            toRun.Add(ShowLightAsync(light));
        }
        return CoUtils.TaskAsRoutine(Task.WhenAll(toRun));
    }

    private async Task ShowLightAsync(Light light) {
        await CoUtils.RunTween(MapManager.Instance.Camera.transform.DOShakePosition(shakeDuration, new Vector3(.05f, .3f, .05f), 10, 30));
        await Task.Delay((int)(pauseDuration * 1000));
        await CoUtils.RunTween(light.DOIntensity(endIntensity, intensityDuration).SetEase(Ease.OutCubic));
        for (var i = 0; i < flickerCount; i += 1) {
            await CoUtils.RunTween(light.DOIntensity(0, flickerDuration).SetEase(Ease.Linear));
            await CoUtils.RunTween(light.DOIntensity(endIntensity, flickerDuration).SetEase(Ease.Linear));
        }
        Global.Instance.Data.SetSwitch("night1_lever_2", true);
        await Task.Delay((int)(midWait * 1000));
        await CoUtils.RunTween(light.DOIntensity(0f, cooldownDuration).SetEase(Ease.OutCubic));
    }
}
