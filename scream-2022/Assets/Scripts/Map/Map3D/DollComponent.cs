using FMODUnity;
using UnityEngine;

public class DollComponent : MonoBehaviour {

    public new SpriteRenderer renderer;
    public SpriteRenderer highlightRenderer;
    public new CapsuleCollider collider;
    public StudioEventEmitter emitter;
    public CharaEvent parent;
    public GameObject offsetter;
}