using UnityEngine;

/// <summary>
/// Turns on alpha when hero is too near
/// </summary>
public class HeroZAlphaBehavior : MonoBehaviour {

    public new SpriteRenderer renderer = null;
    public new Collider collider = null;
    public float minDist;
    public float maxDist;
    public CharaEvent chara;
    public float fadeSpeed = 5f;
    public float sightMax = 0f;

    private float sightMod;

    void Update() {

        if (chara != null) {
            var d = (chara.IsVisible() ? 1 : -1) * Time.deltaTime * fadeSpeed;
            sightMod = Mathf.Clamp(sightMod + d, 0, 1);
        }

        AvatarEvent avatar = Global.Instance.Maps.Avatar;
        var dist = Vector3.Distance(transform.position, avatar.FPSCam.transform.position) - sightMod * sightMax;
        float t;
        if (maxDist - minDist > 0.0f) {
            float clampZ = Mathf.Clamp(dist, minDist, maxDist);
            t = (clampZ - minDist) / (maxDist - minDist);
        } else {
            t = dist > maxDist ? 1 : 0;
        }



        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, t);
        if (collider != null) {
            collider.enabled = dist > .6f;
        }
    }
}
