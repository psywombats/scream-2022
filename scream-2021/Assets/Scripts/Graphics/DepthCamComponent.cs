using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

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

    protected void Start() {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        if (!staticInited) {
            staticInited = true;
            if (blitMat != null) {
                blitMat.SetFloat("_XFade", 0);
            }
        }
    }

    protected void Update() {
        rangeMult = Mathf.MoveTowards(rangeMult, RangeMultTarget, Time.deltaTime * rangeMultRate);
    }

    public override IEnumerator FadeRoutine(FadeData fade, bool invert = false, float timeMult = 1) {
        var elapsed = 0f;
        while (elapsed < fade.delay) {
            if (blitMat != null) {
                var val = elapsed / fade.delay;
                if (invert) val = 1f - val;
                blitMat.SetFloat("_XFade", val);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (blitMat != null) {
            var val = invert ? 0 : 1;
            blitMat.SetFloat("_XFade", val);
        }
    }

    protected void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if (blitMat != null) {
            blitMat.SetFloat("_UniversalEnable", glitchOn ? 1 : 0);
            blitMat.SetFloat("_DLimitMin", rangeMin * rangeMult);
            blitMat.SetFloat("_DLimitMax", rangeMax);
            Graphics.Blit(src, dest, blitMat);
        } else {
            Graphics.Blit(src, dest);
        }
        if (depthDumpMat != null) {
            Graphics.Blit(src, depthDumpTex, depthDumpMat);
        }
    }
}
