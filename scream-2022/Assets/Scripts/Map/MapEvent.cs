using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Dispatch))]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class MapEvent : MonoBehaviour {
    
    private const string PropertyCondition = "show";
    private const string PropertyInteract = "onInteract";
    private const string PropertyCollide = "onCollide";

    public const string EventEnabled = "enabled";
    public const string EventCollide = "collide";
    public const string EventInteract = "interact";
    public const string EventMove = "move";

    [SerializeField] public Vector2Int size = new Vector2Int(1, 1);
    [Space]
    [Header("Movement")]
    [SerializeField] public float tilesPerSecond = 2.0f;
    [SerializeField] public bool passable = true;
    [SerializeField] public OrthoDir requiredDir = OrthoDir.North;
    [Space]
    [Header("Lua scripting")]
    [SerializeField] [TextArea(3, 6)] public string luaCondition;
    [SerializeField] [TextArea(3, 6)] public string luaOnInteract;
    [SerializeField] [TextArea(3, 6)] public string luaOnCollide;
    [SerializeField] private GameObject enableChild;
  
    public bool IsTracking { get; private set; }
    private float lastCollided;
    
    public Vector3 PositionPx {
        get { return transform.localPosition; }
        set {
            transform.localPosition = value;
        }
    }

    public Vector2Int Location {
        get => WorldPositionToTileCoords(PositionPx);
        set {
            transform.localPosition = new Vector3(value.x, Map.Terrain.HeightAt(Location), value.y);
            PositionPx = transform.localPosition;
        }
    }

    private Map parent;
    public Map Map {
        get {
            // this is wiped in update but we'll cache it across frames anyway
            if (parent != null) {
                return parent;
            }
            GameObject parentObject = gameObject;
            while (parentObject.transform.parent != null) {
                parentObject = parentObject.transform.parent.gameObject;
                Map map = parentObject.GetComponent<Map>();
                if (map != null) {
                    parent = map;
                    return map;
                }
            }
            return null;
        }
    }

    private ObjectLayer layer;
    public ObjectLayer Layer {
        get {
            if (layer == null) {
                GameObject parent = gameObject;
                do {
                    parent = parent.transform.parent.gameObject;
                    ObjectLayer objLayer = parent.GetComponent<ObjectLayer>();
                    if (objLayer != null) {
                        layer = objLayer;
                        break;
                    }
                } while (parent.transform.parent != null);
            }
            return layer;
        }
    }

    private bool isSwitchEnabled = true;
    public bool IsSwitchEnabled {
        get {
            return isSwitchEnabled;
        }
        set {
            if (value != isSwitchEnabled) {
                GetComponent<Dispatch>().Signal(EventEnabled, value);
                if (enableChild != null) {
                    enableChild.SetActive(value);
                }
            }
            isSwitchEnabled = value;
        }
    }

  public LuaMapEvent LuaObject { get; private set; }

    public void Awake() {
        LuaObject = new LuaMapEvent(this);
    }

    public void Start() {
        if (Application.isPlaying) {
            LuaObject.Set(PropertyCollide, luaOnCollide);
            LuaObject.Set(PropertyInteract, luaOnInteract);
            LuaObject.Set(PropertyCondition, luaCondition);

            GetComponent<Dispatch>().RegisterListener(EventCollide, (object payload) => {
                OnCollide((AvatarEvent)payload);
            });
            GetComponent<Dispatch>().RegisterListener(EventInteract, (object payload) => {
                OnInteract((AvatarEvent)payload);
            });

            CheckEnabled();
        }
    }

    public virtual void Update() {
        if (Application.IsPlaying(this)) {
            CheckEnabled();
        }
    }

    public void OnDrawGizmos() {
        //if (Selection.activeGameObject == gameObject) {
        //    Gizmos.color = Color.red;
        //} else {
            Gizmos.color = Color.magenta;
        //}
        DrawGizmoSelf();
    }

    public void CheckEnabled() {
        if (luaCondition != null && luaCondition.Length > 0) {
            IsSwitchEnabled = LuaObject.EvaluateBool(PropertyCondition, true);
        }
    }

    public bool IsPassableBy(MapEvent other) {
        return passable || !IsSwitchEnabled;
    }

    public OrthoDir DirectionTo(MapEvent other) {
        return DirectionTo(other.PositionPx);
    }
    public OrthoDir DirectionTo(Vector3 position) {
        return OrthoDirExtensions.DirectionOf3D(position - PositionPx);
    }

    public bool CanPassAt(Vector2Int loc) {
        throw new NotImplementedException();
    }

    public bool ContainsPosition(Vector3 pos) {
        var p1 = PositionPx + new Vector3(-.5f, 0 , -.5f);
        var p2 = p1 + new Vector3(size.x * Map.UnitsPerTile, 0, size.y * Map.UnitsPerTile);
        var res =   pos.x >= p1.x && /*pos.y >= p1.y &&*/ pos.z >= p1.z &&
                    pos.x <= p2.x && /*pos.y <= p2.y &&*/ pos.z <= p2.z;
        if (res == true)
            return true;
        else return false;
    }
    
    public void SetLocation(Vector2Int location) {
        parent = null;
        Location = location;
        SetScreenPositionToMatchTilePosition();
        SetDepth();
    }

    public void SetSize(Vector2Int size) {
        this.size = size;
        SetScreenPositionToMatchTilePosition();
        SetDepth();
    }

    public Vector3 TileToWorldCoords(Vector2Int position) {
        return new Vector3(position.x, Map.Terrain.HeightAt(position), position.y);
    }

    public static Vector2Int WorldPositionToTileCoords(Vector3 pos) {
        return new Vector2Int(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.z));
    }

    public Vector2Int OffsetForTiles(OrthoDir dir) {
        return dir.XY3D();
    }

    public void SetScreenPositionToMatchTilePosition() {
        transform.localPosition = new Vector3(Location.x, Map.Terrain.HeightAt(Location), Location.y);
        PositionPx = transform.localPosition;
    }

    public Vector3 InternalPositionToDisplayPosition(Vector3 position) {
        return position;
    }

    public void SetDepth() {
        // our global height is identical to the height of the parent layer
        if (Map != null) {
            transform.localPosition = new Vector3(
                gameObject.transform.localPosition.x,
                Map.Terrain.HeightAt(Location),
                gameObject.transform.localPosition.z);
        }
    }

    public float CalcTilesPerSecond() {
        return tilesPerSecond;
    }

    public IEnumerator StepMultiRoutine(OrthoDir dir, int steps) {
        var target = PositionPx + dir.Px3D() * steps;
        return LinearStepRoutine(target);
    }

    public IEnumerator LinearStepRoutine(Vector2Int target) {
        return LinearStepRoutine(TileToWorldCoords(target));
    }
    public IEnumerator LinearStepRoutine(Vector3 target) {
        IsTracking = true;
        var elapsed = 0f;
        var goal = (target - transform.localPosition).magnitude / tilesPerSecond * 2f * 1.2f;
        var map = Map;
        while (transform.localPosition != target && elapsed < goal) {
            if (Map != map) break;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, tilesPerSecond * 1.2f * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        IsTracking = false;
    }

    protected void DrawGizmoSelf() {
        if (GetComponent<Map3DHandleExists>() != null) {
            return;
        }
        var fudge = 0.005f;
        if (gameObject.name.Contains("target")) {
            Gizmos.color = new Color(.66f, 1f, 0f, 0.5f);
            fudge *= 2;
        } else if (gameObject.name.Contains("trigger")) {
            Gizmos.color = new Color(0, .5f, 1f, 0.5f);
        } else {
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
        }
        Gizmos.DrawCube(new Vector3(
                transform.position.x + size.x * OrthoDir.East.Px3DX() / 2.0f,
                transform.position.y,
                transform.position.z + size.y * OrthoDir.North.Px3DZ() / 2.0f),
            new Vector3((size.x - 0.1f), fudge, (size.y - 0.1f)));
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(
                transform.position.x + size.x * OrthoDir.East.Px3DX() / 2.0f,
                transform.position.y,
                transform.position.z + size.y * OrthoDir.North.Px3DZ() / 2.0f),
            new Vector3((size.x - 0.1f), fudge, (size.y - 0.1f)));
    }

  // called when the avatar stumbles into us
  // before the step if impassable, after if passable
  private bool triggered = false;
    private void OnCollide(AvatarEvent avatar) {
    if (avatar.InputPaused)
    {
      return;
    }
        if (luaOnCollide.Length == 0) {
            return;
        }
        if (Time.time - lastCollided < .5f) {
            return;
        }
        lastCollided = Time.time;
        LuaObject.Run(PropertyCollide);
    }

    // called when the avatar stumbles into us
    // facing us if impassable, on top of us if passable
    private void OnInteract(AvatarEvent avatar) {
        LuaObject.Run(PropertyInteract);
    }

    private LuaScript ParseScript(string lua) {
        if (lua == null || lua.Length == 0) {
            return null;
        } else {
            return new LuaScript(GetComponent<LuaContext>(), lua);
        }
    }

    private LuaCondition ParseCondition(string lua) {
        if (lua == null || lua.Length == 0) {
            return null;
        } else {
           return GetComponent<LuaContext>().CreateCondition(lua);
        }
    }
}
