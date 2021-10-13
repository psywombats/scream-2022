using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

/// <summary>
/// Turns on alpha when hero is too near
/// </summary>
public class HeroZAlphaBehavior : MonoBehaviour {

    public float minZ;
    public float maxZ;

    void Update() {
        AvatarEvent avatar = Global.Instance.Maps.Avatar;
        float z = avatar.transform.localPosition.z;
        float t;
        if (maxZ - minZ > 0.0f) {
            float clampZ = Mathf.Clamp(z, minZ, maxZ);
            t = (maxZ - clampZ) / (maxZ - minZ);
        } else {
            t = z > maxZ ? 1 : 0;
        }
        

        if (GetComponent<TilemapRenderer>() != null) {
            GetComponent<TilemapRenderer>().material.SetColor("_Color", new Color(1, 1, 1, t));
        } else {
            GetComponent<MeshRenderer>().enabled = t > 0.5f;
        }
    }
}
