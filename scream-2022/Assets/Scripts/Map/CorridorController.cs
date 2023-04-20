using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;

[RequireComponent(typeof(Map))]
public class CorridorController : MonoBehaviour, IComparer<PanelLightComponent> {

    private const float width = 18f;
    private const int MaxChunks = 5;

    [SerializeField] private TacticsTerrainMesh chunkPrefab;
    [SerializeField] private GameObject cork1;
    [SerializeField] private GameObject cork2;

    public List<PanelLightComponent> allLights = new List<PanelLightComponent>();

    public int Bias { get; set; } = 0;

    public bool DefaultShutdown { get; private set; }
    public bool DefaultEvil { get; private set; }

    private Map map;
    public Map Map => map ?? (map = GetComponent<Map>());

    private int pass;

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
        UpdateTerrain();
        UpdateLighting();
    }

    public async Task RunRoutineAsync(string routineName) {
        switch (routineName) {
            case "pt1a":
                await Task.Delay(8 * 1000);
                await RandomSwapAsync();
                await Task.Delay(5 * 1000);
                await RandomShutdownAsync();
                await Task.Delay(3 * 1000);

                var fade = IndexDatabase.Instance.Fades.GetData("black");
                AvatarEvent.Instance.PauseInput();
                await MapManager.Instance.Camera.fade.FadeRoutine(fade, invert: false, .1f);
                MapManager.Instance.Teleport("Gazer", "pt1a", OrthoDir.North, isRaw: true);
                MapOverlayUI.Instance.adv.SetWake(0);
                await Task.Delay(2 * 1000);
                await MapManager.Instance.Camera.fade.FadeRoutine(fade, invert: true, 2.5f);
                AvatarEvent.Instance.UnpauseInput();

                await MapManager.Instance.Lua.RunRoutineFromFile("pt1_02");

                
                break;
        }
    }

    public async Task RandomSwapAsync(float duration = 1f) {
        DefaultEvil = true;
        var randomLights = allLights.OrderBy(a => Guid.NewGuid()).ToList();
        foreach (var light in randomLights) {
            light.IsEvil = true;
            await Task.Delay((int)(1000 * duration / randomLights.Count()));
        }
    }

    public async Task RandomShutdownAsync(float duration = 2.5f) {
        DefaultShutdown = true;
        var randomLights = allLights.OrderBy(a => Guid.NewGuid()).ToList();
        foreach (var light in randomLights) {
            light.IsShutDown = true;
            await Task.Delay((int)(1000 * duration / randomLights.Count()));
        }
    }

    private void UpdateTerrain() {
        TacticsTerrainMesh bestChunk = null;
        var bestDist = 0f;
        var posX = AvatarEvent.Instance.transform.position.x;
        var toRemove = new List<int>();
        foreach (var pair in chunks) {
            var chunk = pair.Value;
            var dist = Mathf.Abs(posX - (chunk.transform.position.x + 9));
            if (dist > width * MaxChunks + 1) {
                Destroy(chunk);
                toRemove.Add(pair.Key);
            }
            else if (bestChunk == null || dist < bestDist) {
                bestDist = dist;
                bestChunk = chunk;
            }
        }
        Map.terrain = bestChunk;
        foreach (var tr in toRemove) {
            chunks.Remove(tr);
        }

        var normalized = posX / width;
        EnsureChunk(Mathf.FloorToInt(normalized + pass + Bias));
        pass += 1;
        if (pass >= Mathf.FloorToInt(MaxChunks / 2)) {
            pass = -1 * Mathf.FloorToInt(MaxChunks / 2);
        }
    }

    public void UpdateLighting() {
        allLights.Sort(this);
        var count = 0;

        if (allLights.Count < 8) {
            foreach (var light in allLights) {
                light.IsLimited = false;
            }
        } else {
            foreach (var light in allLights) {
                if (light.checker.isVisible) {
                    light.IsLimited = count >= 8;
                    count += 1;
                }  else {
                    light.IsLimited = true;
                }
            }
        }
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

    public int Compare(PanelLightComponent x, PanelLightComponent y) {
        var avatar = AvatarEvent.Instance;
        var a = Vector3.Distance(x.transform.position, avatar.transform.position);
        var b = Vector3.Distance(y.transform.position, avatar.transform.position);
        return Mathf.RoundToInt(1000 * (a - b));
    }
}
