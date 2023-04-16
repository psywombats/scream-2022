using DG.Tweening;
using System.Collections;
using UnityEngine;

public class NightAlertController : MonoBehaviour {

    [SerializeField] private new Light light = null;
    [Space]
    [SerializeField] private float delay1 = 4f;
    [SerializeField] private float delay2 = 12f;
    [SerializeField] private float intensity = 1f;
    [SerializeField] private float intensityIn = .5f;

    public IEnumerator AlertRoutine() {
        Global.Instance.Data.SetSwitch("night2_alert", true);
        yield return CoUtils.Wait(delay1);
        yield return CoUtils.RunTween(light.DOIntensity(intensity, intensityIn));
        light.GetComponent<LightOscillator>().enabled = true;
        yield return CoUtils.Wait(delay2);
        var ev = AvatarEvent.Instance.Event;
        ev.LuaObject.Set("alert", "play('night2_alert')");
        ev.LuaObject.Run("alert");
        AvatarEvent.Instance.DisableHeightCrossing = false;
    }
}