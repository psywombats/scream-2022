using UnityEngine;

public class LightEnvironment : MonoBehaviour {

    [SerializeField] private float ambientMult = 0f;

    public void OnEnable() {
//#if UNITY_WEBGL
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientEquatorColor = new Color(ambientMult, ambientMult, ambientMult);
        RenderSettings.ambientGroundColor = new Color(ambientMult, ambientMult, ambientMult);
        RenderSettings.ambientSkyColor = new Color(ambientMult, ambientMult, ambientMult);
//#else
//        RenderSettings.ambientIntensity = ambientMult;
 //       RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
//#endif
    }
}
