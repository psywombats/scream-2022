using UnityEngine;

public class RandomWalkComponent : MonoBehaviour {

    [SerializeField] private Vector3 min;
    [SerializeField] private Vector3 max;
    [SerializeField] private Vector3 veloMax;
    [SerializeField] private Vector3 veloGain;

    private Vector3 Velocity;

    public void Update() {
        Velocity += new Vector3(
            veloGain.x * Random.Range(-1f, 1f),
            veloGain.y * Random.Range(-1f, 1f),
            veloGain.z * Random.Range(-1f, 1f));
        Velocity.x = Mathf.Clamp(Velocity.x, -veloMax.x, veloMax.x);
        Velocity.y = Mathf.Clamp(Velocity.y, -veloMax.y, veloMax.y);
        transform.localPosition += Velocity * Time.deltaTime;
        transform.localPosition = new Vector3(
            Mathf.Clamp(transform.localPosition.x, min.x, max.x),
            Mathf.Clamp(transform.localPosition.y, min.y, max.y),
            Mathf.Clamp(transform.localPosition.z, min.z, max.z));
    }
}