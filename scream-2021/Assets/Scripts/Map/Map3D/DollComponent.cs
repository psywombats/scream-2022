using UnityEngine;

public class DollComponent : MonoBehaviour {
    public new SpriteRenderer renderer;
    public new CapsuleCollider collider;

    protected void Start() {
        if (AvatarEvent.Instance.UseFirstPersonControl) {
            renderer.transform.localPosition = new Vector3(.5f, 0, .5f);
        } else {
            renderer.transform.localPosition = new Vector3(.5f, 0, .12f);
        }
    }
}