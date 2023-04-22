using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DepthCamComponent : FadeComponent {

    private static bool staticInited;

    [SerializeField] private Material blitMat = null;
    [SerializeField] private Material depthDumpMat = null;
    [SerializeField] private RenderTexture depthDumpTex = null;
    [Space]
    [SerializeField] private bool glitchOn = false;
    [SerializeField] public float rangeMin = 10f;
    [SerializeField] public float rangeMax = 12f;

    public bool GlitchFromSwitch { get; set; }

    private float rangeMult = 1f;
    public float RangeMultTarget { get; set; } = 1f;
    private const float rangeMultRate = .05f;

    private static float fader;

    protected void Start() {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        if (!staticInited) {
            staticInited = true;
            if (blitMat != null) {
                fader = Application.isEditor ? 0 : 1;
                blitMat.SetFloat("_XFade", fader);
            }
        }
    }

    protected void Update() {
        GlitchFromSwitch = Global.Instance.Data.GetSwitch("glitch_on");
        rangeMult = Mathf.MoveTowards(rangeMult, RangeMultTarget, Time.deltaTime * rangeMultRate);
        blitMat.SetFloat("_UniversalEnable", (GlitchFromSwitch || glitchOn) ? 1 : 0);
        blitMat.SetFloat("_DLimitMin", rangeMin * rangeMult);
        blitMat.SetFloat("_DLimitMax", rangeMax * rangeMult);
    }

    public override IEnumerator FadeRoutine(FadeData fade, bool invert = false, float timeMult = 1) {
        MapOverlayUI.Instance.xfade.DOFade(invert ? 0 : 1, timeMult * fade.delay).Play();
        return CoUtils.Wait(timeMult * fade.delay);
        /*
        var elapsed = 0f;
        var origFade = fader;
        var target = invert ? 0 : 1;
        while (elapsed < fade.delay) {
            if (blitMat != null) {
                var t = elapsed / fade.delay;
                fader = origFade * (1f - t) + target * t;
                blitMat.SetFloat("_XFade", fader);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (blitMat != null) {
            blitMat.SetFloat("_XFade", target);
            fader = target;
        }
        */

    }

    /*
    protected void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if (blitMat != null) {
            blitMat.SetFloat("_UniversalEnable", (GlitchFromSwitch || glitchOn) ? 1 : 0);
            blitMat.SetFloat("_DLimitMin", rangeMin * rangeMult);
            blitMat.SetFloat("_DLimitMax", rangeMax * rangeMult);
            Graphics.Blit(src, dest, blitMat);
        } else {
            Graphics.Blit(src, dest);
        }
        if (depthDumpMat != null) {
            Graphics.Blit(src, depthDumpTex, depthDumpMat);
        }
    }
    */
}
