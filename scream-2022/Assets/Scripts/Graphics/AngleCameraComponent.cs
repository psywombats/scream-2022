using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AngleCameraComponent : MonoBehaviour {

    [SerializeField] private float angle = 15f;
    [SerializeField] private float cutoffX = 5;
    [SerializeField] private float maxAngleInX = 5;
    

    protected void Update() {
        if (angle < 0 && transform.localPosition.x < cutoffX || angle > 0 && transform.localPosition.x > cutoffX) {
            var delta = Mathf.Abs(transform.localPosition.x - cutoffX);
            var take = delta / maxAngleInX;
            if (take > 1) take = 1;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle * take, transform.localEulerAngles.z);
        }
    }
}