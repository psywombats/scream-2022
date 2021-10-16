using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DepthCamComponent : FadeComponent {

    private static bool staticInited;

    [SerializeField] private Material blitMat;
    [SerializeField] private Material depthDumpMat;
    [SerializeField] private RenderTexture depthDumpTex;

    public void Start() {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        if (!staticInited) {
            staticInited = true;
            if (blitMat != null) {
                blitMat.SetFloat("_XFade", 0);
            }
        }
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
            Graphics.Blit(src, dest, blitMat);
        } else {
            Graphics.Blit(src, dest);
        }
        if (depthDumpMat != null) {
            Graphics.Blit(src, depthDumpTex, depthDumpMat);
        }
    }
}
