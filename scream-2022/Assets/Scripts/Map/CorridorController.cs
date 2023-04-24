using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Map))]
public class CorridorController : MonoBehaviour, IComparer<PanelLightComponent> {

    private const float width = 18f;
    private const int MaxChunks = 5;

    [SerializeField] private TacticsTerrainMesh chunkPrefab;
    [SerializeField] private GameObject cork1;
    [SerializeField] private GameObject cork2;
    [Space]
    [SerializeField] private bool introMode;
    [SerializeField] private bool deathMode;
    [SerializeField] private bool winMode;
    [SerializeField] private bool sumiMode;
    [Space]
    [SerializeField] private GameObject monsterPrefab;

    public List<PanelLightComponent> allLights = new List<PanelLightComponent>();

    public int Bias { get; set; } = 0;

    public bool DefaultShutdown { get; private set; }
    public bool DefaultEvil { get; private set; }

    private Map map;
    public Map Map => map ?? (map = GetComponent<Map>());

    private int pass;
    private bool stopGeneration;
    private float awaitingX;

    private Dictionary<int, TacticsTerrainMesh> chunks = new Dictionary<int, TacticsTerrainMesh>();
    public void Start() {
        chunks.Add(0, Map.Terrain);

        if (deathMode || winMode) {
            DefaultShutdown = true;
            DefaultEvil = true;
        }
        if (deathMode) {
            StartCoroutine(RunDeathRoutine());
        }

        if (winMode) {
            StartCoroutine(RunWinRoutine());
        }

        if (sumiMode) {
            DefaultShutdown = true;
            StartCoroutine(RunSumiRoutine());
        }
    }

    public void OnDisable() {
        if (AvatarEvent.Instance != null) {
            AvatarEvent.Instance.FreeTraverse = false;
        }
    }

    public void Update() {
        AvatarEvent.Instance.FreeTraverse = true;
        if (!stopGeneration) {
            UpdateTerrain();
        }
        UpdateLighting();
    }

    public IEnumerator RunIntroRoutine(string routineName) {

        yield return CoUtils.Wait(7f);
        yield return RandomSwapRoutine(1.5f);
        yield return CoUtils.Wait(5f);
        yield return RandomShutdownRoutine(3.2f);
        yield return CoUtils.Wait(3f);

        yield return GoToGazerRoutine();
        yield return MapManager.Instance.Lua.RunRoutineFromFile("pt1_02");

    }

    private IEnumerator RunDeathRoutine() {

        yield return CoUtils.Wait(3f);
        yield return OrderedTurnOnRoutine();
        yield return CoUtils.Wait(6f);

        var mobj = Instantiate(monsterPrefab);
        mobj.transform.SetParent(Map.transform);
        mobj.transform.position = new Vector3(AvatarEvent.Instance.transform.position.x + MonsterController.MaxDist, 0, 3.5f);
        var monster = mobj.GetComponent<MonsterController>();

        yield return CoUtils.Wait(30f);

        stopGeneration = true;
        var bestX = 0f;
        foreach (var chunk in chunks) {
            if (bestX == 0f || chunk.Value.transform.position.x < bestX) {
                bestX = chunk.Value.transform.position.x;
            }
        }

        awaitingX = bestX  + 2f;
        yield return HitXRoutine();

        monster.Unbound = true;
        yield return CoUtils.Wait(3f);
        awaitingX = bestX  + 1f;
        yield return HitXRoutine();

        monster.Dying = true;
        AvatarEvent.Instance.PauseInput();
        var targetPos = new Vector3(bestX, 0f, 3.5f);
        var dir = (targetPos - AvatarEvent.Instance.FPSCam.transform.position).normalized;
        var lookAngles = Quaternion.LookRotation(dir).eulerAngles;

        AvatarEvent.Instance.FPSCam.transform.DORotate(lookAngles, MonsterController.DeathTime).Play();
        
        yield return MapOverlayUI.Instance.death.RunRoutine();
        MapOverlayUI.Instance.adv.SetWake(0);
        yield return CoUtils.Wait(2f);

        MapManager.Instance.Teleport("Gazer", "chair", OrthoDir.South, isRaw: true);

        MapOverlayUI.Instance.death.gameObject.SetActive(false);
        AvatarEvent.Instance.UnpauseInput();

        yield return MapManager.Instance.Lua.RunRoutineFromFile("pt2_01");

        AvatarEvent.Instance.UnpauseInput();
    }

    private IEnumerator GoToGazerRoutine() {
        var fade = IndexDatabase.Instance.Fades.GetData("black");
        AvatarEvent.Instance.PauseInput();
        yield return MapManager.Instance.Camera.fade.FadeRoutine(fade, invert: false, .1f);
        MapManager.Instance.Teleport("Gazer", "chair", OrthoDir.South, isRaw: true);
        MapOverlayUI.Instance.adv.SetWake(0);
        yield return CoUtils.Wait(2f);
        yield return MapManager.Instance.Camera.fade.FadeRoutine(fade, invert: true, 2.5f);
        AvatarEvent.Instance.UnpauseInput();
    }

    private IEnumerator RunWinRoutine() {
        yield return CoUtils.Wait(3f);
        yield return OrderedTurnOnRoutine();
        yield return CoUtils.Wait(3f);
        yield return Global.Instance.Maps.Lua.RunRoutineFromFile("finale_kowalski");
    }

    private IEnumerator RunSumiRoutine() {
        UpdateTerrain();
        yield return CoUtils.Wait(.1f);
        yield return OrderedTurnOnRoutine();
        stopGeneration = true;
    }

    public IEnumerator OrderedTurnOnRoutine() {
        DefaultShutdown = false;
        var orderedLights = allLights.OrderBy(a => Vector3.Distance(AvatarEvent.Instance.transform.position, a.transform.position)).ToList();
        for (var i = 0; i < orderedLights.Count; i += 1) {
            var lgroup = new List<PanelLightComponent>();
            lgroup.Add(orderedLights[i]);
            i += 1;
            if (i < orderedLights.Count) lgroup.Add(orderedLights[i]);
            i += 1;
            if (i < orderedLights.Count) lgroup.Add(orderedLights[i]);
            i += 1;
            if (i < orderedLights.Count) lgroup.Add(orderedLights[i]);
            
            foreach (var light in lgroup) {
                light.IsShutDown = false;
                light.PlayBootupSFX();
                yield return CoUtils.Wait(.05f);
            }
            yield return CoUtils.Wait(.8f);
        }
    }

    public IEnumerator RandomSwapRoutine(float duration = 1f) {
        DefaultEvil = true;
        var randomLights = allLights.OrderBy(a => Guid.NewGuid()).ToList();
        foreach (var light in randomLights) {
            light.IsEvil = !light.IsEvil;
            yield return CoUtils.Wait(duration / randomLights.Count());
        }
    }

    public IEnumerator RandomShutdownRoutine(float duration = 2.5f, bool startup = false) {
        DefaultShutdown = true;
        var randomLights = allLights.OrderBy(a => Guid.NewGuid()).ToList();
        foreach (var light in randomLights) {
            light.IsShutDown = !startup;
            light.PlayBootupSFX();
            yield return CoUtils.Wait(duration / randomLights.Count());
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

    private IEnumerator HitXRoutine() {
        while (AvatarEvent.Instance.transform.position.x > awaitingX) {
            yield return null;
        }
    }
}
