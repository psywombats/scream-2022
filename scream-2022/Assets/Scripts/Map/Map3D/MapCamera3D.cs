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
        //lastFixedZ = transform.position.z;
        //GetComponent<Camera>().transparencySortMode = TransparencySortMode.Orthographic;
    }

    public void LateUpdate() {
        ManualUpdate();
    }

    public override void ManualUpdate() {
        base.ManualUpdate();
        
    }

    [Serializable]
    public struct AngleConstraint {
        public float angle;
        public float cutoffX;
        public float maxAngleInX;
    }
}
