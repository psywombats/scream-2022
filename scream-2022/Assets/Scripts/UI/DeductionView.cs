using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DeductionView : UnityEngine.MonoBehaviour {
    
    [SerializeField] private List<DeductionButton> buttons;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private GameObject blocker;

    private TaskCompletionSource<string> tcs;

    public void Start() {
        foreach (var button in buttons) {
            button.button.onClick.AddListener(() =>
            {
                HandleCommand(button.command);
            });
        }
    }

    public async Task<string> DoMenuAsync() {
        await CoUtils.RunTween(canvas.DOFade(1f, 1f));
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        blocker.SetActive(false);
        tcs = new TaskCompletionSource<string>();
        await tcs.Task;
        blocker.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        var res = tcs.Task.Result;
        await CoUtils.RunTween(canvas.DOFade(0f, 1f));
        return res;
    }

    public void HandleCommand(string cmd) {
        if (tcs != null) {
            tcs.SetResult(cmd);
        }
    }
}
