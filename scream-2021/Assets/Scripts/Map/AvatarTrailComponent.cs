using UnityEngine;

[RequireComponent(typeof(CharaEvent))]
[RequireComponent(typeof(Rigidbody))]
public class AvatarTrailComponent : MonoBehaviour {

    [SerializeField] private float desiredDistance = 2f;
    [SerializeField] private float runawayDistance = 0f;
    [SerializeField] private float phengoDistance = 0f;
    [SerializeField] private new Collider collider = null;

    private MapEvent @event;
    private MapEvent Event => @event ?? (@event = GetComponent<MapEvent>());

    private Rigidbody body;
    private Rigidbody Body => body ?? (body = GetComponent<Rigidbody>());

    private Vector3 velocityAcc;

    protected void Update() {
        if (!Event.IsSwitchEnabled) return;
        velocityAcc /= 2f;
        var target = AvatarEvent.Instance.Event.PositionPx;
        var delta = target - Event.PositionPx;

        if (phengoDistance > 0 && GetComponent<CharaEvent>().IsVisible() && delta.sqrMagnitude < phengoDistance * phengoDistance) {
            var v = -1 * delta.normalized * Event.tilesPerSecond;
            velocityAcc += v / 2f;
        } if (desiredDistance > 0 && delta.sqrMagnitude > desiredDistance * desiredDistance) {
            var v = delta.normalized * Event.tilesPerSecond;
            velocityAcc += v / 2f;
        } else if (runawayDistance > 0 && delta.sqrMagnitude < runawayDistance * runawayDistance) {
            var v = -1 * delta.normalized * Event.tilesPerSecond;
            velocityAcc += v / 2f;
        }
        
        
        if (collider != null && !collider.enabled) {
            velocityAcc = default;
            Body.velocity = default;
        } else {
            Body.velocity = velocityAcc;
        }
    }
}