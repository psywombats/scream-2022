using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public DeductionView deduction;
    public SettingBox setting;
    public NVLComponent adv;
    public Image xfade;
}
