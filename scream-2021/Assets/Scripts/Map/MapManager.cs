using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class MapManager : SingletonBehavior {

    public static MapManager Instance => Global.Instance.Maps;

    public AvatarEvent Avatar { get; set; }

    private Map activeMap;
    public Map ActiveMap {
        get => activeMap;
        set {
            activeMap = value;
            activeMapName = activeMap.MapName;
        }
    }
   

    private new MapCamera camera;
    public MapCamera Camera {
        get {
            if (camera == null) {
                camera = FindObjectOfType<MapCamera>();
            }
            return camera;
        }
        set {
            camera = value;
        }
    }

    private LuaContext lua;
    public LuaContext Lua {
        get {
            if (lua == null) {
                lua = new LuaCutsceneContext();
                lua.Initialize();
            }
            return lua;
        }
    }

    private string activeMapName;

    public IEnumerator TeleportRoutine(string mapName, Vector2Int location, OrthoDir? facing = null, bool isRaw = false) {
        Avatar?.PauseInput();
        TransitionData data = IndexDatabase.Instance.Transitions.GetData(FadeComponent.DefaultTransitionTag);
        if (!isRaw) {
            yield return Camera.fade.FadeRoutine(data.GetFadeOut());
            RawTeleport(mapName, location, facing);
            Camera = activeMap.GetComponentInChildren<MapCamera>();
            yield return Camera.fade.FadeRoutine(data.GetFadeIn(), true);
        } else {
            RawTeleport(mapName, location, facing);
            yield return CoUtils.Wait(0.1f);
        }
        Avatar.UnpauseInput();
    }

    public IEnumerator TeleportRoutine(string mapName, string targetEventName, OrthoDir? facing = null, bool isRaw = false) {
        bool avatarExists = Avatar != null;
        if (avatarExists) Avatar.PauseInput();
        TransitionData data = IndexDatabase.Instance.Transitions.GetData(FadeComponent.DefaultTransitionTag);
        if (!isRaw) {
            yield return Camera.fade.FadeRoutine(data.GetFadeOut());
            RawTeleport(mapName, targetEventName, facing);
            Camera = activeMap.GetComponentInChildren<MapCamera>();
            yield return Camera.fade.FadeRoutine(data.GetFadeIn(), true);
        } else {
            RawTeleport(mapName, targetEventName, facing);
            yield return CoUtils.Wait(0.1f);
        }
        if (avatarExists) Avatar.UnpauseInput();
    }

    private void RawTeleport(string mapName, Vector2Int location, OrthoDir? facing = null) {
        Map newMapInstance = InstantiateMap(mapName);
        RawTeleport(newMapInstance, location, facing);
    }

    private void RawTeleport(string mapName, string targetEventName, OrthoDir? facing = null) {
        Map newMapInstance;
        if (mapName == activeMapName) {
            newMapInstance = ActiveMap;
        } else {
            newMapInstance = InstantiateMap(mapName);
        }
        activeMapName = mapName;
        MapEvent target = newMapInstance.GetEventNamed(targetEventName);
        AvatarEvent.Instance.Chara.renderer.color = Color.white;
        if (target == null) {
            Debug.LogError("Couldn't find target " + targetEventName + " on " + mapName + " from " + activeMapName);
            RawTeleport(newMapInstance, Avatar.Event.Location, facing);
        } else {
            RawTeleport(newMapInstance, target.Location, facing);
        }
    }

    private void RawTeleport(Map map, Vector2Int location, OrthoDir? facing = null) {
        activeMapName = null;
        if (Avatar == null) {
            AddInitialAvatar(map);
        } else {
            Avatar.transform.SetParent(map.ObjectLayer.transform, false);
        }

        if (map != ActiveMap) {
            if (ActiveMap != null) {
                ActiveMap.OnTeleportAway(map);
            }
        }

        Avatar.GetComponent<MapEvent>().transform.position = new Vector3(location.x, map.Terrain.HeightAt(location), location.y);
        if (facing != null) {
            Avatar.Chara.Facing = facing.GetValueOrDefault(OrthoDir.North);
        }

        if (map != ActiveMap) {
            if (ActiveMap != null) {
                Destroy(ActiveMap.gameObject);
            }

            ActiveMap = map;
            ActiveMap.OnTeleportTo();
        }
    }

    private Map InstantiateMap(string mapName) {
        GameObject newMapObject = null;
        if (ActiveMap != null) {
            string localPath = Map.ResourcePath + mapName;
            newMapObject = Resources.Load<GameObject>(localPath);
        }
        if (newMapObject == null) {
            newMapObject = Resources.Load<GameObject>(mapName);
        }
        Assert.IsNotNull(newMapObject);
        var obj = Instantiate(newMapObject);
        obj.transform.position = Vector3.zero;
        return obj.GetComponentInChildren<Map>();
    }

    private void AddInitialAvatar(Map map) {
        Avatar = FindObjectOfType<AvatarEvent>();
        Avatar.transform.SetParent(map.ObjectLayer.transform, false);
        Camera.target = Avatar.Event;
        Camera.ManualUpdate();
    }
}
