using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Tilemap))]
public class TacticsTerrainMesh : MonoBehaviour, ISerializationCallbackReceiver {
    
    [HideInInspector] public Vector2Int size;
    [HideInInspector] public float[] heights;
    [HideInInspector] public string paletteName;

    [SerializeField] public string meshName;
    [SerializeField] public Tile defaultTopTile;
    [SerializeField] public Tile defaultFaceTile;

    [HideInInspector] public FacingTileDictionary serializedFacingTiles;
    private Dictionary<FacingTileKey, Tile> facingTiles = new Dictionary<FacingTileKey, Tile>();

    private Tilemap _tilemap;
    public Tilemap tilemap {
        get {
            if (_tilemap == null) _tilemap = GetComponent<Tilemap>();
            return _tilemap;
        }
    }

    protected void Start() {
        var collider = GetComponent<MeshCollider>();
        if (collider != null) {
            collider.sharedMesh = GetComponent<MeshFilter>().mesh;
        }
    }

    public void Fix() {
        
    }

    public void Resize(Vector2Int newSize) {
        List<FacingTileKey> toRemove = new List<FacingTileKey>();
        foreach (FacingTileKey key in facingTiles.Keys) {
            if (key.pos.x >= newSize.x || key.pos.z >= newSize.y) {
                toRemove.Add(key);
            }
        }
        foreach (FacingTileKey key in toRemove) {
            facingTiles.Remove(key);
        }

        float[] newHeights = new float[newSize.x * newSize.y];
        Tile[] newTiles = new Tile[newSize.x * newSize.y];
        for (int y = 0; y < (size.y < newSize.y ? size.y : newSize.y); y += 1) {
            for (int x = 0; x < (size.x < newSize.x ? size.x : newSize.x); x += 1) {
                newHeights[y * newSize.x + x] = heights[y * size.x + x];
                newTiles[y * newSize.x + x] = tilemap.GetTile<Tile>(new Vector3Int(x, y, 0));
            }
        }
        for (int y = size.y; y < newSize.y; y += 1) {
            for (int x = size.x; x < newSize.x; x += 1) {
                newHeights[y * size.x + x] = 0.5f;
            }
        }

        tilemap.ClearAllTiles();
        for (int y = 0; y < (size.y < newSize.y ? size.y : newSize.y); y += 1) {
            for (int x = 0; x < (size.x < newSize.x ? size.x : newSize.x); x += 1) {
                tilemap.SetTile(new Vector3Int(x, y, 0), newTiles[y * (size.x < newSize.x ? size.x : newSize.x) + x]);
            }
        }
        heights = newHeights;
        size = newSize;
    }

    public float HeightAt(Vector2 pos) {
        var x1 = Mathf.FloorToInt(pos.x + .33f);
        var x2 = Mathf.CeilToInt(pos.x - .33f);
        var y1 = Mathf.FloorToInt(pos.y + .33f);
        var y2 = Mathf.CeilToInt(pos.y - .33f);

        var h1 = HeightAt(x1, y1);
        var h2 = HeightAt(x1, y2);
        var ha = Mathf.Lerp(h1, h2, pos.y - y1);

        var h3 = HeightAt(x2, y1);
        var h4 = HeightAt(x2, y2);
        var hb = Mathf.Lerp(h3, h4, pos.y - y1);

        return Mathf.Lerp(ha, hb, pos.x - x1);
    }
    public float HeightAt(Vector2Int pos) {
        return HeightAt(pos.x, pos.y);
    }
    public float HeightAt(int x, int y) {
        if (x < 0 || x >= size.x || y < 0 || y >= size.y) {
            return -5;
        } else {
            return heights[y * size.x + x];
            
        }
    }

    public void SetHeight(int x, int y, float height) {
        heights[y * size.x + x] = height;
    }

    public Tile TileAt(int x, int y) {
        Tile tile = tilemap.GetTile<Tile>(new Vector3Int(x, y, 0));
        if (tile == null) {
            return defaultTopTile;
        } else {
            return tile;
        }
    }

    public Tile TileAt(int x, int y, float height, OrthoDir dir) {
        FacingTileKey key = new FacingTileKey();
        key.dir = dir;
        key.pos = new Vector3(x, height, y);
        if (facingTiles.ContainsKey(key) && facingTiles[key] != null) {
            return facingTiles[key];
        } else {
            return defaultFaceTile;
        }
    }

    public void SetTile(int x, int y, Tile tile) {
        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
    }

    public void SetTile(int x, int y, float height, OrthoDir dir, Tile tile) {
        FacingTileKey key = new FacingTileKey();
        key.dir = dir;
        key.pos = new Vector3(x, height, y);
        facingTiles[key] = tile;
    }

    // === SERIALIZATION ===========================================================================

    public void OnBeforeSerialize() {
        serializedFacingTiles = new FacingTileDictionary(facingTiles);
    }

    public void OnAfterDeserialize() {
        if (serializedFacingTiles != null) {
            facingTiles = serializedFacingTiles.ToDictionary();
        } else {
            facingTiles = new Dictionary<FacingTileKey, Tile>();
        }
    }

    [System.Serializable]
    public struct FacingTileKey {
        public Vector3 pos;
        public OrthoDir dir;
    }

    [System.Serializable]
    public class FacingTileDictionary : SerialDictionary<FacingTileKey, Tile> {
        public FacingTileDictionary(Dictionary<FacingTileKey, Tile> dictionary) : base(dictionary) {

        }
    }
}
