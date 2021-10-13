using UnityEngine;

public class MapCamera : MonoBehaviour {

    [SerializeField] public MapEvent target;
    [SerializeField] public FadeComponent fade = null;

    // these are read by sprites, not actually enforced by the cameras
    [SerializeField] public bool billboardX;
    [SerializeField] public bool billboardY;

    public virtual void ManualUpdate() {

    }

    public virtual Camera GetCameraComponent() {
        return GetComponent<Camera>();
    }
}
