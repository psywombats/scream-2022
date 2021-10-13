﻿using UnityEngine;
using System.Collections;

public class TrackerCam3D : MapCamera3D {
    
    public Vector3 targetOffset;

    void Start() {
        if (target == null) {
            target = AvatarEvent.Instance.Event;
        }
    }

    public override void ManualUpdate() {
        if (target != null) {
            transform.position = target.transform.position + targetOffset;
            base.ManualUpdate();
        }
    }

    public void MemorizePosition() {
        targetOffset = transform.position - target.transform.position;
    }
}