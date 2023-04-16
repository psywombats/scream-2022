using UnityEngine;

public class TrackerComponent : MonoBehaviour {
    
    public Vector3 targetOffset;

    public bool fixedZ;
    public bool constrain;
    public Vector3 minValues = new Vector3(0, 0, 0);
    public Vector3 maxValues = new Vector3(100, 100, 0);

    private float lastFixedZ;
    private MapEvent target;

    void Start() {
        if (target == null) {
            target = FindObjectOfType<AvatarEvent>()?.Event;
        }
    }

    public void Awake() {
        lastFixedZ = transform.position.z;
    }

    public void LateUpdate() {
        ManualUpdate();
    }

    public void ManualUpdate() {
        if (target != null) {
            transform.position = target.transform.position + targetOffset;
        }
        if (fixedZ) {
            transform.position = new Vector3(transform.position.x, transform.position.y, lastFixedZ);
        }
        if (constrain) {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minValues.x, maxValues.x),
                Mathf.Clamp(transform.position.y, minValues.y, maxValues.y),
                transform.position.z);
        }
    }

    public void MemorizePosition() {
        if (target == null) {
            target = FindObjectOfType<AvatarEvent>()?.Event;
        }
        targetOffset = transform.position - target.transform.position;
    }
}
