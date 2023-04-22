using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class DollComponent : MonoBehaviour {

    public List<SpriteRenderer> renderers;
    public SpriteRenderer highlightRenderer;
    public new CapsuleCollider collider;
    public StudioEventEmitter emitter;
    public CharaEvent parent;
    public GameObject offsetter;
}