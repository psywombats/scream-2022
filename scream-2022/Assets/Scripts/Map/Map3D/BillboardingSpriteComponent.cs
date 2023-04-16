using UnityEngine;

/**
 * Renders the attached sprite as fixed-x at the camera.
 */
//[ExecuteInEditMode]
[DisallowMultipleComponent]
public class BillboardingSpriteComponent : MonoBehaviour {

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
        if (!AvatarEvent.Instance.UseFirstPersonControl) {
            Vector3 angles = transform.eulerAngles;
            transform.eulerAngles = new Vector3(
                    GetCamera().GetCameraComponent().transform.eulerAngles.x,
                    angles.y,
                    angles.z);
        }
        if (AvatarEvent.Instance.UseFirstPersonControl) {
            Vector3 angles = transform.eulerAngles;
            var origX = angles.x;
            var origY = angles.z;
            transform.LookAt(AvatarEvent.Instance.FPSCam.transform, Vector3.up);
            transform.eulerAngles = new Vector3(
                    angles.x,
                    transform.eulerAngles.y - 180,
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
