using UnityEngine;

public class MapCamera : MonoBehaviour {

    [SerializeField] public MapEvent target;
    [SerializeField] public FadeComponent fade = null;

    // these are read by sprites, not actually enforced by the cameras
    [SerializeField] private bool billboardX;
    [SerializeField] private bool billboardY;
    [Space]
    [SerializeField] public Oscillator osc;

    public Camera Cam => GetComponent<Camera>();

    public virtual void ManualUpdate() {

    }
}
