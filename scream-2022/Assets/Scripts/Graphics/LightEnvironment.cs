using UnityEngine;

public class LightEnvironment : MonoBehaviour {

    [SerializeField] private float ambientMult = 0f;

    public void OnEnable() {
        RenderSettings.ambientIntensity = ambientMult;
    }
}
