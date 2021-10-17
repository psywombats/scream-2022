using UnityEngine;

[RequireComponent(typeof(CharaEvent))]
[RequireComponent(typeof(Rigidbody))]
public class AvatarTrailComponent : MonoBehaviour {

    [SerializeField] private float desiredDistance = 2f;

    private MapEvent @event;
    private MapEvent Event => @event ?? (@event = GetComponent<MapEvent>());

    private Rigidbody body;
    private Rigidbody Body => body ?? (body = GetComponent<Rigidbody>());

    protected void Update() {
        var target = AvatarEvent.Instance.Event.PositionPx;
        var delta = target - Event.PositionPx;
        if (delta.sqrMagnitude > desiredDistance * desiredDistance) {
            var v = delta.normalized * Event.tilesPerSecond;
            Body.velocity = v;
        } else {
            Body.velocity = Vector3.zero;
        }
    }
}