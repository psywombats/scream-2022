using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DepthCamComponent: MonoBehaviour {

    [SerializeField] private Material blitMat;
    [SerializeField] private Material depthDumpMat;
    [SerializeField] private RenderTexture depthDumpTex;

    CommandBuffer keepDepthTexture;

    public void Start() {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
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
