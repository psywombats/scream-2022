using DG.Tweening;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntertitleController : MonoBehaviour {

    private const string SfxParameter = "intertitle_sentence finish";

    [SerializeField] private List<IntertitleBar> bars = null;
    [SerializeField] private CanvasGroup canvas = null;
    [Space]
    [SerializeField] private float delay = .7f;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float interleaveDuration = .8f;
    [Space]
    [SerializeField] private StudioEventEmitter sfx = null;

    public void Autostart() {
        foreach (var bar in bars) {
            StartCoroutine(bar.FadeInRoutine(.3f));
        }
    }

    public IEnumerator FadeInRoutine() {
        var toRun = new List<IEnumerator>();
        foreach (var bar in bars) {
            toRun.Add(bar.FadeInRoutine(fadeDuration));
        }
        yield return CoUtils.RunParallel(this, toRun.ToArray());
    }

    public IEnumerator FadeOutRoutine() {
        var toRun = new List<IEnumerator>();
        foreach (var bar in bars) {
            toRun.Add(bar.FadeOutRoutine(fadeDuration));
        }
        yield return CoUtils.RunParallel(this, toRun.ToArray());
    }

    public IEnumerator FadeOutAllRoutine() {
        var toRun = new List<IEnumerator>();
        foreach (var bar in bars) {
            toRun.Add(bar.FadeOutAllRoutine(fadeDuration));
        }
        yield return CoUtils.RunParallel(this, toRun.ToArray());
        sfx.Stop();
    }

    public IEnumerator DisplayRoutine(string text) {
        AudioManager.Instance.BaseVolume = 1f;
        AudioManager.Instance.SetVolume();
        gameObject.SetActive(true);
        yield return CoUtils.RunTween(canvas.DOFade(1f, fadeDuration));
        yield return CoUtils.Wait(interleaveDuration);
        sfx.Play();
        yield return FadeInRoutine();

        var lines = new List<string>(text.Split('\n'));

        var flip = false;
        while (lines.Count < bars.Count) {
            if (flip) {
                lines.Add("");
            } else {
                lines.Insert(0, "");
            }
            flip = !flip;
        }

        for (var i = 0; i < lines.Count; i += 1) {
            var words = lines[i].Split();
            var bar = bars[i];
            for (var j = 0; j < words.Length; j += 1) { 
                yield return CoUtils.Wait(delay);
                var fullLen = ToGo(j, words);
                bar.AddFix(words[j], fullLen);
            }
            if (words.Length == 0) {
                yield return CoUtils.Wait(delay);
            }
        }
        
        yield return CoUtils.Wait(interleaveDuration);
        sfx.SetParameter(SfxParameter, 1);
        yield return FadeOutRoutine();
        yield return CoUtils.Wait(interleaveDuration * 4);
        sfx.SetParameter(SfxParameter, 2);
        yield return FadeOutAllRoutine();
        yield return CoUtils.RunTween(canvas.DOFade(0f, fadeDuration));
        gameObject.SetActive(false);
        sfx.Stop();
    }

    private int ToGo(int from, string[] words) {
        var len = 0;
        for (var i = from; i < words.Length; i += 1) {
            len += words[i].Length;
            len += 1;
        }
        return len;
    } 
}
