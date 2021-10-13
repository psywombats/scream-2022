using UnityEngine;
using UnityEngine.UI;

// empty flag mostly, used by saga scene manager
public class SceneRootComponent : MonoBehaviour {

    public GameSceneManager.SceneType type;
    
    //[SerializeField] private CanvasScaler scaler = null;
    //[SerializeField] private Canvas canvas = null;

    //public void FitToScene(SceneCamera scene) {
    //    canvas.renderMode = RenderMode.ScreenSpaceCamera;
    //    canvas.worldCamera = scene.cam;
    //    scaler.scaleFactor = 1;
    //    LayoutRebuilder.ForceRebuildLayoutImmediate(canvas.GetComponent<RectTransform>());
    //}
}
