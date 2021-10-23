using UnityEngine;

[RequireComponent(typeof(CharaEvent))]
[RequireComponent(typeof(Rigidbody))]
public class AvatarTrailComponent : MonoBehaviour {

    [SerializeField] private float desiredDistance = 2f;
    [SerializeField] private float runawayDistance = 0f;

    private MapEvent @event;
    private MapEvent Event => @event ?? (@event = GetComponent<MapEvent>());

    private Rigidbody body;
    private Rigidbody Body => body ?? (body = GetComponent<Rigidbody>());

    protected void Update() {
        var target = AvatarEvent.Instance.Event.PositionPx;
        var delta = target - Event.PositionPx;
        if (desiredDistance > 0) {
            if (delta.sqrMagnitude > desiredDistance * desiredDistance) {
                var v = delta.normalized * Event.tilesPerSecond;
                Body.velocity = v;
            } else {
                Body.velocity = Vector3.zero;
            }
        }
        if (runawayDistance > 0) {
            if (delta.sqrMagnitude < runawayDistance * runawayDistance) {
                var v = -1 * delta.normalized * Event.tilesPerSecond;
                Body.velocity = v;
            } else {
                Body.velocity = Vector3.zero;
            }
        }
    }
}