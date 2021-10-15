using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntertitleController : MonoBehaviour {

    [SerializeField] private List<IntertitleBar> bars = null;
    [Space]
    [SerializeField] private float delay = .7f;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float interleaveDuration = .8f;

    protected void Start() {
        StartCoroutine(DisplayRoutine("VIOLENCE IS NOT\nTHE ANSWER\n\nVIOLENCE IS THE QUESTION\n\nTHE ANSWER\nIS YES"));
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
    }

    public IEnumerator DisplayRoutine(string text) {
        yield return CoUtils.Wait(interleaveDuration);
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
        yield return FadeOutRoutine();
        yield return CoUtils.Wait(interleaveDuration * 2);
        yield return FadeOutAllRoutine();
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
