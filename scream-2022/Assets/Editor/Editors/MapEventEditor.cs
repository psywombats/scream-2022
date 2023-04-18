using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapEvent), true)]
public class MapEventEditor : Editor {

    private static readonly string DollPath = "Assets/Resources/Prefabs/MapEvent/Doll.prefab";

    public override void OnInspectorGUI() {
        MapEvent mapEvent = (MapEvent)target;

        if (GUI.changed) {
            mapEvent.SetScreenPositionToMatchTilePosition();
            mapEvent.SetDepth();
        }

        Vector2Int newPosition = EditorGUILayout.Vector2IntField("Tiles position", mapEvent.Location);
        if (newPosition != mapEvent.Location) {
            mapEvent.transform.hasChanged = false;
            mapEvent.SetLocation(newPosition);
        }

        base.OnInspectorGUI();
    }

    public void OnSceneGUI() {
        MapEvent mapEvent = (MapEvent)target;
        var handlePos = Handles.PositionHandle(((MapEvent)target).transform.position, Quaternion.identity);
        ((MapEvent)target).transform.position = new Vector3(
            Mathf.RoundToInt(handlePos.x * 2) / 2f,
            handlePos.y,
            Mathf.RoundToInt(handlePos.z * 2) / 2f);
        mapEvent.SetDepth();
    }
}
