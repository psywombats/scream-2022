using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MapOverlayUI : MonoBehaviour {

    private static MapOverlayUI _instance;
    private static Scene _lastScene;
    public static MapOverlayUI Instance {
        get {
            Scene scene = SceneManager.GetActiveScene();
            if (_lastScene != scene) {
                _lastScene = scene;
                _instance = null;
            }
            if (_instance == null) {
                _instance = FindObjectOfType<MapOverlayUI>();
            }
            return _instance;
        }
    }

    public SpeakboxComponent Textbox;
    public IntertitleController Intertitle;
    public SettingBox Setting;
    public NotebookBox Notes;
    public CardController Cards;
    public KeywordController Keywords;
    public CalDeathController Pupils;
    public CanvasGroup endgamer;
    public CanvasGroup subendgamer;
    public ControlController controls;
    public ClippyController clippy;
}
