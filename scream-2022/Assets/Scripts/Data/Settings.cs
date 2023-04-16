using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Data/Settings/Settings")]
public class Settings : ScriptableObject {

    public FontData systemFont;

    public static Settings Instance() {
        return Resources.Load<Settings>("Database/Settings");
    }
}
