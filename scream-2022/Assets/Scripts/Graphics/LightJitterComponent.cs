using UnityEngine;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(Oscillator))]
public class LightJitterComponent : MonoBehaviour {

    private new Light light;
    public Light Light => light ?? (light = GetComponent<Light>());

    private Oscillator osc;
    public Oscillator Osc => osc ?? (osc = GetComponent<Oscillator>());

    [SerializeField] private float delta;
    [SerializeField] private float targetIntensity;

    public void Start() {
        Osc.Elapsed = Random.Range(0, 10f);
    }

    public void Update() {
        Light.intensity = targetIntensity + delta * Osc.CalcVectorMult();
    }
}

