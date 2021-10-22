using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Map : MonoBehaviour {

    /// <summary>The number of pixels a tile takes up</summary>
    public const int PxPerTile = 16;
    /// <summary>The number of pixels that make up a tile</summary>
    public const float UnitsPerTile = 1;

    public const string ResourcePath = "Maps/";

    [SerializeField] private string mapName = "New map";
    [SerializeField] private Grid grid = null;
    [SerializeField] private ObjectLayer objectLayer = null;
    [SerializeField] private TacticsTerrainMesh terrain = null;
    [Space]
    [SerializeField] private string bgmKey = null;
    [SerializeField] private List<string> settings = null;
    [SerializeField] private float glitchRangeMin = 10;
    [SerializeField] private float glitchRangeMax = 12f;
    
    // true if the tile in question is passable at x,y
    private Dictionary<Tilemap, bool[,]> passabilityMap;

    private Vector2Int _size;
    public Vector2Int size {
        get {
            if (_size.x == 0) {
                if (Terrain != null) {
                    _size = GetComponent<TacticsTerrainMesh>().size;
                } else {
                    Vector3Int v3 = grid.transform.GetChild(0).GetComponent<Tilemap>().size;
                    _size = new Vector2Int(v3.x, v3.y);
                }
            }
            return _size;
        }
    }

    public string MapName => mapName;
    public TacticsTerrainMesh Terrain => terrain;
    public ObjectLayer ObjectLayer => objectLayer;

    public Vector2 SizePx { get { return size * PxPerTile; } }
    public int Width { get { return size.x; } }
    public int Height { get { return size.y; } }

    private List<Tilemap> layers;
    public List<Tilemap> Layers {
        get {
            if (layers == null) {
                layers = new List<Tilemap>();
                if (Terrain != null) {
                    layers.Add(GetComponent<Tilemap>());
                } else {
                    foreach (Transform child in grid.transform) {
                        if (child.GetComponent<Tilemap>()) {
                            layers.Add(child.GetComponent<Tilemap>());
                        }
                    }
                }
            }
            return layers;
        }
    }

    public void Start() {
        // TODO: figure out loading
        if (Global.Instance.Maps.ActiveMap == null) {
            Global.Instance.Maps.ActiveMap = this;
        }
    }

    public Vector3Int TileToTilemapCoords(Vector2Int loc) {
        return TileToTilemapCoords(loc.x, loc.y);
    }

    public Vector3Int TileToTilemapCoords(int x, int y) {
        return new Vector3Int(x, -1 * (y + 1), 0);
    }

    public PropertiedTile TileAt(Tilemap layer, int x, int y) {
        return (PropertiedTile)layer.GetTile(TileToTilemapCoords(x, y));
    }

    public bool IsChipPassableAt(Tilemap layer, Vector2Int loc) {
        if (passabilityMap == null) {
            passabilityMap = new Dictionary<Tilemap, bool[,]>();
        }
        if (!passabilityMap.ContainsKey(layer)) {
            passabilityMap[layer] = new bool[Width, Height];
            for (int x = 0; x < Width; x += 1) {
                for (int y = 0; y < Height; y += 1) {
                    PropertiedTile tile = TileAt(layer, x, y);
                    passabilityMap[layer][x, y] = tile == null || tile.GetData().passable;
                }
            }
        }

        return passabilityMap[layer][loc.x, loc.y];
    }
    
    public List<MapEvent> GetEventsAt(Vector3 pos) {
        List<MapEvent> events = new List<MapEvent>();
        foreach (MapEvent mapEvent in objectLayer.GetComponentsInChildren<MapEvent>()) {
            if (mapEvent.ContainsPosition(pos)) {
                events.Add(mapEvent);
            }
        }
        return events;
    }

    // returns the first event at loc that implements T
    public T GetEventAt<T>(Vector3 pos) {
        var events = GetEventsAt(pos);
        foreach (var mapEvent in events) {
            if (mapEvent.GetComponent<T>() != null) {
                return mapEvent.GetComponent<T>();
            }
        }
        return default;
    }

    // returns all events that have a component of type t
    public List<T> GetEvents<T>() {
        return new List<T>(objectLayer.GetComponentsInChildren<T>());
    }

    public Tilemap TileLayerAtIndex(int layerIndex) {
        return GetComponentsInChildren<Tilemap>()[layerIndex];
    }

    public MapEvent GetEventNamed(string eventName) {
        foreach (ObjectLayer layer in GetComponentsInChildren<ObjectLayer>()) {
            foreach (MapEvent mapEvent in layer.GetComponentsInChildren<MapEvent>(includeInactive: true)) {
                if (mapEvent.name == eventName && mapEvent.IsSwitchEnabled) {
                    return mapEvent;
                }
            }
        }
        return null;
    }

    private static bool firstMap = true;
    public void OnTeleportTo() {
        if (bgmKey != null) {
            AudioManager.Instance.PlayBGM(bgmKey);
        }
        if (firstMap == true) {
            firstMap = false;
        } else {
            firstMap = false;
            foreach (var setting in settings) {
                MapOverlayUI.Instance.Setting.Show(setting);
            }
        }
        if (glitchRangeMin > 0) {
            var cam = AvatarEvent.Instance.FPSCam.GetCameraComponent().GetComponent<DepthCamComponent>();
            cam.rangeMin = glitchRangeMin;
            cam.rangeMax = glitchRangeMax;
        }
    }

    public void OnTeleportAway(Map nextMap) {

    }

    // returns a list of coordinates to step to with the last one being the destination, or null
    public List<Vector2Int> FindPath(MapEvent actor, Vector2Int to) {
        return FindPath(actor, to, Width > Height ? Width : Height);
    }
    public List<Vector2Int> FindPath(MapEvent actor, Vector2Int to, int maxPathLength) {
        if (ManhattanDistance(actor.GetComponent<MapEvent>().Location, to) > maxPathLength) {
            return null;
        }
        if (!actor.CanPassAt(to)) {
            return null;
        }

        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        List<List<Vector2Int>> heads = new List<List<Vector2Int>>();
        List<Vector2Int> firstHead = new List<Vector2Int>();
        firstHead.Add(actor.GetComponent<MapEvent>().Location);
        heads.Add(firstHead);

        while (heads.Count > 0) {
            heads.Sort(delegate (List<Vector2Int> pathA, List<Vector2Int> pathB) {
                int pathACost = pathA.Count + ManhattanDistance(pathA[pathA.Count - 1], to);
                int pathBCost = pathB.Count + ManhattanDistance(pathB[pathB.Count - 1], to);
                return pathACost.CompareTo(pathBCost);
            });
            List<Vector2Int> head = heads[0];
            heads.RemoveAt(0);
            Vector2Int at = head[head.Count - 1];

            if (at == to) {
                // trim to remove the current location from the beginning
                return head.GetRange(1, head.Count - 1);
            }

            if (head.Count < maxPathLength) {
                foreach (OrthoDir dir in Enum.GetValues(typeof(OrthoDir))) {
                    Vector2Int next = head[head.Count - 1];
                    // minor perf here, this is critical code
                    switch (dir) {
                        case OrthoDir.East:     next.x += 1;    break;
                        case OrthoDir.North:    next.y += 1;    break;
                        case OrthoDir.West:     next.x -= 1;    break;
                        case OrthoDir.South:    next.y -= 1;    break;
                    }
                    if (!visited.Contains(next) && actor.CanPassAt(next) &&
                        (actor.GetComponent<CharaEvent>() == null ||
                             actor.CanPassAt(next)) &&
                        (actor.GetComponent<CharaEvent>() == null ||
                             actor.GetComponent<CharaEvent>().CanCrossTileGradient(at, next))) {
                        List<Vector2Int> newHead = new List<Vector2Int>(head) { next };
                        heads.Add(newHead);
                        visited.Add(next);
                    }
                }
            }
        }

        return null;
    }

    private static int ManhattanDistance(Vector2Int a, Vector2Int b) {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
