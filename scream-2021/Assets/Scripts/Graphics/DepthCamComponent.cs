using System.Collections;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DepthCamComponent : FadeComponent {

    private static bool staticInited;

    [SerializeField] private Material blitMat = null;
    [SerializeField] private Material depthDumpMat = null;
    [SerializeField] private RenderTexture depthDumpTex = null;
    [Space]
    [SerializeField] private bool glitchOn = false;
    [SerializeField] private float rangeMin = 10f;
    [SerializeField] private float rangeMax = 12f;

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
        rangeMult = Mathf.MoveTowards(rangeMult, RangeMultTarget, Time.deltaTime * rangeMultRate);
    }

    public override IEnumerator FadeRoutine(FadeData fade, bool invert = false, float timeMult = 1) {
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
    }

    protected void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if (blitMat != null) {
            blitMat.SetFloat("_UniversalEnable", glitchOn ? 1 : 0);
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
}
