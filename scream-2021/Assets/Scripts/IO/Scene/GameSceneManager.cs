using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections;

public class GameSceneManager : SingletonBehavior, IInputListener {

    public enum SceneType {
        Map3D
    }

    public enum WindowMode {
        Fullscreen,
        Windowed,
    }

    public enum FrameMode {
        None,
        HUD,
        Centered,
    }

    public bool Loading { get; private set; }
    public SceneType CurrentSceneType => sceneStack.Peek().type;
    public WindowMode CurrentWindowMode => (WindowMode)Global.Instance.SystemData.SettingWindow.Value;

    public event Action<SceneType> OnSceneChange;
    public event Action<WindowMode> OnWindowChanged;

    private Stack<SceneRootComponent> sceneStack = new Stack<SceneRootComponent>();
    
    public void Start() {
        Global.Instance.Serialization.SystemData.SettingWindow.OnModify += () => {
            SwitchWindowMode((WindowMode)Global.Instance.SystemData.SettingWindow.Value);
        };
        SwitchWindowMode((WindowMode)Global.Instance.SystemData.SettingWindow.Value);
    }

    public bool OnCommand(InputManager.Command command, InputManager.Event eventType) {
        // while we're active, we eat all input
        return true;
    }

    public async Task PopAsync() {
        var last = sceneStack.Pop();
        await SceneManager.UnloadSceneAsync(last.type.ToString());
        OnSceneChange?.Invoke(sceneStack.Peek().type);
        sceneStack.Peek().enabled = true;
    }

    public async Task<SceneRootComponent> LoadAsync(SceneType scene, bool additive = false, bool disableScene = false) {
        var sceneName = scene.ToString();
        Loading = true;
        if (!disableScene) {
            Global.Instance.Input.PushListener(this);
        }
        
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        var routines = new List<Coroutine>();
        OnSceneChange?.Invoke(scene);
        if (sceneStack.Count > 0) {
            var last = sceneStack.Peek();
            last.enabled = false;
            if (!additive) {
                while (sceneStack.Count > 0) {
                    var last2 = sceneStack.Pop();
                    await SceneManager.UnloadSceneAsync(last2.type.ToString());
                }
            }
        }
        var sceneRoot = FindObjectsOfType<SceneRootComponent>().Where(obj => obj.enabled).First();
        //var sceneCam = FindObjectOfType<SceneCamera>();
        //sceneRoot.FitToScene(sceneCam);
        sceneStack.Push(sceneRoot);
        if (disableScene) {
            sceneRoot.gameObject.SetActive(false);
        }

        Loading = false;
        if (!disableScene) {
            Global.Instance.Input.RemoveListener(this);
        }

        return sceneRoot;
    }

    // meant to be called in response to settings changes
    private void SwitchWindowMode(WindowMode mode) {
        switch (mode) {
            case WindowMode.Fullscreen:
                Resolution biggest = Screen.resolutions.FirstOrDefault();
                var resolutions = Screen.resolutions;
                foreach (var res in resolutions) {
                    if (res.width * res.height > biggest.width * biggest.height) {
                        biggest = res;
                    }
                }
                Screen.SetResolution(biggest.width, biggest.height, FullScreenMode.FullScreenWindow);
                break;
            case WindowMode.Windowed:
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                break;
        }
        StartCoroutine(SwitchedWindowRoutine(mode));
    }

    private IEnumerator SwitchedWindowRoutine(WindowMode mode) {
        yield return null;
        OnWindowChanged?.Invoke(mode);
    }
}
