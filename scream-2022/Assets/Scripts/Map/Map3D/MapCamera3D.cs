using System;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera3D : MapCamera {

    public bool fixedZ;
    public bool constrain;
    public Vector3 minValues = new Vector3(0, 0, 0);
    public Vector3 maxValues = new Vector3(100, 100, 0);
    [Space]
    public List<AngleConstraint> constraints;

    private float lastFixedZ;

    public void Awake() {
        lastFixedZ = transform.position.z;
        //GetComponent<Camera>().transparencySortMode = TransparencySortMode.Orthographic;
    }

    public void LateUpdate() {
        ManualUpdate();
    }

    public override void ManualUpdate() {
        base.ManualUpdate();
        if (fixedZ) {
            transform.position = new Vector3(transform.position.x, transform.position.y, lastFixedZ);
        }
        if (constrain) {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minValues.x, maxValues.x),
                Mathf.Clamp(transform.position.y, minValues.y, maxValues.y),
                transform.position.z);
        }

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
        foreach (var c in constraints) {
            if (c.angle < 0 && transform.localPosition.x < c.cutoffX || c.angle > 0 && transform.localPosition.x > c.cutoffX) {
                var delta = Mathf.Abs(transform.localPosition.x - c.cutoffX);
                var take = delta / c.maxAngleInX;
                if (take > 1) take = 1;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, c.angle * take, transform.localEulerAngles.z);
                transform.localPosition = new Vector3(c.cutoffX, transform.localPosition.y, transform.localPosition.z);
            }
        }
    }

    [Serializable]
    public struct AngleConstraint {
        public float angle;
        public float cutoffX;
        public float maxAngleInX;
    }
}
