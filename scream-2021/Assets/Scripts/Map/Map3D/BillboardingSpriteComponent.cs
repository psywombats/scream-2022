using UnityEngine;

/**
 * Renders the attached sprite as fixed-x at the camera.
 */
//[ExecuteInEditMode]
[DisallowMultipleComponent]
public class BillboardingSpriteComponent : MonoBehaviour {

    public bool billboardX = true;
    public bool billboardY;

    protected void Update() {
        Billboard();
    }

    protected void OnValidate() {
        //Billboard();
    }

    public void Billboard() {
        if (GetCamera() == null) {
            return;
        }
        if (billboardX || GetCamera().billboardX) {
            Vector3 angles = transform.eulerAngles;
            transform.eulerAngles = new Vector3(
                    GetCamera().GetCameraComponent().transform.eulerAngles.x,
                    angles.y,
                    angles.z);
        }
        if (billboardY || GetCamera().billboardY) {
            Vector3 angles = transform.eulerAngles;
            transform.eulerAngles = new Vector3(
                    angles.x,
                    GetCamera().GetCameraComponent().transform.eulerAngles.y,
                    angles.z);
        }
    }

    private MapCamera GetCamera() {
        if (Application.isPlaying) {
            return Global.Instance.Maps.Camera;
        } else {
            return FindObjectOfType<MapCamera>();
        }
    }
}
