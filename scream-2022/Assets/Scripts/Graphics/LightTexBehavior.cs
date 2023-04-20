using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightTexBehavior : MonoBehaviour {

    private const int Quality = 4;

    [SerializeField] private RenderTexture sourceTex;

    private Texture2D dummy;

    private new Light light;
    public Light Light => light ?? (light = GetComponent<Light>());

    public void Start() {
        dummy = new Texture2D(1, 1);
    }

    public void Update() {
        var color = SampleSourceTex(sourceTex);
        Light.color = color;
    }

    private Color SampleSourceTex(RenderTexture sourceTex) {
        RenderTexture.active = sourceTex;
        var r = 0f;
        var g = 0f;
        var b = 0f;
        for (var x = sourceTex.width / Quality / 2f; x <= sourceTex.width; x += sourceTex.width / Quality) {
            for (var y = sourceTex.width / Quality / 2f; y <= sourceTex.width; y += sourceTex.width / Quality) {
                dummy.ReadPixels(new Rect(x, y, 1, 1), 0, 0);
                dummy.Apply();
                var c = dummy.GetPixel(0, 0);
                r += c.r;
                g += c.g;
                b += c.b;
            }
        }
        var sq = Quality * Quality;
        return new Color(r / sq, g / sq, b / sq);
    }
}
