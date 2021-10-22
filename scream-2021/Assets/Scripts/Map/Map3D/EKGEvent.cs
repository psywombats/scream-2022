using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class EKGEvent : MonoBehaviour {

    private const int MaxEmitters = 6;

    [SerializeField] private StudioEventEmitter pulseSFX = null;
    [SerializeField] private StudioEventEmitter alertSFX = null;
    [SerializeField] private StudioEventEmitter flatlineSFX = null;
    [SerializeField] private List<SimpleSpriteAnimator> animators = null;
    [SerializeField] private List<SimpleSpriteAnimator> redliners = null;
    [Space]
    [SerializeField] private float minNext = 3f;
    [SerializeField] private float maxNext = 4f;

    private float toNext;
    private float elapsed;

    private bool amAlertEmitter;
    private bool alerting;
    private bool flatlining;
    private bool checkedAlert;
    private static int alerterCount;

    public void Start() {
        RecalcNext();
        elapsed = Random.Range(0, toNext);
    }

    public void Update() {
        elapsed += Time.deltaTime;
        if (elapsed > toNext) {
            TriggerPulse();
        }
    }

    public void OnDisable() {
        pulseSFX.Stop();
        alertSFX.Stop();
        flatlineSFX.Stop();
    }

    private void TriggerPulse() {
        if (!Global.Instance.Data.GetSwitch("d2_clear")) {
            return;
        }
        if (Global.Instance.Data.GetSwitch("night2_alert") && !alerting) {
            alerting = true;
            if (alerterCount < MaxEmitters) {
                alerterCount += 1;
                amAlertEmitter = true;
            }
            foreach (var animator in animators) {
                animator.enabled = false;
            }
            foreach (var redliner in redliners) {
                redliner.gameObject.SetActive(true);
            }
        }
        if (alerting) {
            if (amAlertEmitter) {
                if (!flatlining && Random.Range(0, 2) == 0) {
                    flatlining = true;
                    alertSFX.Play();
                } else if (!checkedAlert) {
                    checkedAlert = true;
                    alertSFX.Stop();
                    flatlineSFX.Play();
                }
            }
        } else {
            pulseSFX.Play();
            foreach (var animator in animators) {
                StartCoroutine(animator.PlayRoutine());
            }
        }
        
        RecalcNext();
    }

    private void RecalcNext() {
        toNext = Random.Range(minNext, maxNext);
        elapsed = 0;
    }
}
