using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Map))]
public class CorridorController : MonoBehaviour {

    [SerializeField] private TacticsTerrainMesh chunkPrefab;
    [SerializeField] private GameObject cork1;
    [SerializeField] private GameObject cork2;

    private int pass;
    private const float width = 18f;

    private Map map;
    public Map Map => map ?? (map = GetComponent<Map>());

    private Dictionary<int, TacticsTerrainMesh> chunks = new Dictionary<int, TacticsTerrainMesh>();

    public void Start() {
        chunks.Add(0, Map.Terrain);
    }

    public void OnEnable() {
        
    }

    public void OnDisable() {
        AvatarEvent.Instance.FreeTraverse = false;
    }

    public void Update() {
        AvatarEvent.Instance.FreeTraverse = true;
        TacticsTerrainMesh bestChunk = null;
        var bestDist = 0f;
        var posX = AvatarEvent.Instance.transform.position.x;
        var toRemove = new List<int>();
        foreach (var pair in chunks) {
            var chunk = pair.Value;
            var dist = Mathf.Abs(posX - (chunk.transform.position.x + 9));
            if (dist > width * 3) {
                Destroy(chunk);
                toRemove.Add(pair.Key);
            } else if (bestChunk == null || dist < bestDist) {
                bestDist = dist;
                bestChunk = chunk;
            }
        }
        Map.terrain = bestChunk;
        foreach (var tr in toRemove) {
            chunks.Remove(tr);
        }

        var normalized = posX / width;
        EnsureChunk(Mathf.FloorToInt(normalized - 1 + pass));
        pass += 1;
        if (pass >= 4) pass = 0;
    }

    private void EnsureChunk(int index) {
        if (chunks.ContainsKey(index)) {
            return;
        }

        var newChunk = Instantiate(chunkPrefab);
        chunks.Add(index, newChunk);
        newChunk.transform.SetParent(transform);
        newChunk.transform.position = new Vector3(
            width * index,
            0,
            0);

        var indices = new List<int>(chunks.Keys);
        indices.Sort();
        cork1.transform.position = new Vector3(indices[0] * width, 0, 0);
        cork2.transform.position = new Vector3((indices[indices.Count - 1] + 1) * width, 0, 0);
    }
}
