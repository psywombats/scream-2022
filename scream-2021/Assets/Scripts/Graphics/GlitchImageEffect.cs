using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GlitchImageEffect : MonoBehaviour {

    [SerializeField] private Material material;
    [SerializeField] private bool enableGlitch = true;

    private float elapsedSeconds;

    public void Update() {
        AssignCommonShaderVariables();
        elapsedSeconds += Time.deltaTime;
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination) {
        material.SetTexture("_MainTexture", source);
        AssignCommonShaderVariables();
        Graphics.Blit(source, destination, material);
    }

    private void AssignCommonShaderVariables() {
        material.SetFloat("_UniversalEnable", enableGlitch ? 1 : 0);
    }
}
