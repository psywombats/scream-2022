using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapEvent), true)]
public class MapEventEditor : Editor {

    private static readonly string DollPath = "Assets/Resources/Prefabs/Doll.prefab";

    public override void OnInspectorGUI() {
        MapEvent mapEvent = (MapEvent)target;

        if (GUI.changed) {
            //mapEvent.SetScreenPositionToMatchTilePosition();
            //mapEvent.SetDepth();
        }

        if (!mapEvent.GetComponent<CharaEvent>()) {
            if (GUILayout.Button("Add Chara Event")) {
                GameObject dollPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(DollPath);
                var doll = (GameObject)PrefabUtility.InstantiatePrefab(dollPrefab);
                doll.name = mapEvent.name + " (doll)";
                GameObjectUtility.SetParentAndAlign(doll, mapEvent.gameObject);
                CharaEvent chara = mapEvent.gameObject.AddComponent<CharaEvent>();
                chara.renderer = doll.GetComponent<SpriteRenderer>();
                mapEvent.passable = false;
                Undo.RegisterCreatedObjectUndo(mapEvent, "Create " + doll.name);
                Selection.activeObject = doll;

                // hardcode weirdness
                doll.transform.localPosition = new Vector3(Map.UnitsPerTile / 2, 0, 0.25f);
            }
            GUILayout.Space(25.0f);
        }

        Vector2Int newPosition = EditorGUILayout.Vector2IntField("Tiles position", mapEvent.Location);
        if (newPosition != mapEvent.Location) {
            mapEvent.SetLocation(newPosition);
        }

        Vector2Int newSize = EditorGUILayout.Vector2IntField("Size", mapEvent.size);
        if (newSize != mapEvent.size) {
            mapEvent.SetSize(newSize);
        }

        base.OnInspectorGUI();
    }

    public void OnSceneGUI() {
        //((MapEvent)target).transform.position = Handles.PositionHandle(((MapEvent)target).transform.position, Quaternion.identity);
    }

    public void OnEnable() {
        Tools.hidden = true;
    }

    public void OnDisable() {
        Tools.hidden = false;
    }
}
