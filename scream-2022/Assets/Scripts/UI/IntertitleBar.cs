using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class IntertitleBar : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI text;
    [Space]
    [SerializeField] private int characterCount = 45;
    [SerializeField] private float flipTime = .1f;
    [SerializeField] private float redTime = .1f;

    private List<KeyValuePair<string, int>> fixes = new List<KeyValuePair<string, int>>();
    private float[] timings;

    private StringBuilder sb;
    private float redAt;
    private float elapsed;
    private float tElapsed;
    private float fader, outFader;

    private void Start() {
        elapsed = Random.Range(0, flipTime);
        timings = new float[characterCount];
        for (var i = 0; i < characterCount; i += 1) {
            timings[i] = Random.Range(0f, 1f);
        }
        Randomize();
    }

    protected void Update() {
        elapsed += Time.deltaTime * Random.Range(0.8f, 1);
        tElapsed += Time.deltaTime;
        if (elapsed > flipTime) {
            Randomize();
            elapsed %= flipTime;
        }
    }

    public IEnumerator FadeInRoutine(float duration) {
        redAt = 0;
        elapsed = 0;
        tElapsed = 0;
        fader = 0;
        outFader = 0;
        fixes = new List<KeyValuePair<string, int>>();
        while (fader < 1) {
            fader += Time.deltaTime / duration;
            yield return null;
        }
    }

    public IEnumerator FadeOutRoutine(float duration) {
        while (fader > 0) {
            fader -= Time.deltaTime / duration;
            yield return null;
        }
    }

    public IEnumerator FadeOutAllRoutine(float duration) {
        while (outFader < 1) {
            outFader += Time.deltaTime / duration;
            yield return null;
        }
    }

    public void Randomize() {
        if (sb == null) {
            sb = new StringBuilder(characterCount * 2);
        } else {
            sb.Clear();
        }
        var at = 0;
        for (var i = 0; i < fixes.Count; i += 1) {
            var r = (tElapsed - redAt) / (redTime * 2);
            var red = (i == fixes.Count - 1) && (r < 1);
            var incr = fixes[i].Value - at;
            AppendGarbage(sb, incr, at);
            if (red) {
                var rh = (int)(r * 512f - 256f);
                if (rh < 0) rh = 0;
                sb.Append("<color=#ff");
                var str = rh.ToString("X2");
                sb.Append(str);
                sb.Append(str);
                sb.Append(">");
            }
            var word = fixes[i].Key;
            for (var j = 0; j < word.Length; j += 1) {
                if (outFader < timings[at + j]) {
                    sb.Append(word[j]);
                } else {
                    sb.Append(' ');
                }
            }
            if (red) { sb.Append("</color>"); }
            at += incr + fixes[i].Key.Length;
        }
        AppendGarbage(sb, characterCount - at, at);
        text.text = sb.ToString();
    }

    public void AddFix(string word, int fullLength) {
        redAt = tElapsed;
        var at = 0;
        if (fixes.Count > 0) {
            at = fixes[fixes.Count - 1].Value + fixes[fixes.Count - 1].Key.Length;
        }
        var to = Random.Range(at + 1, characterCount - fullLength - 1);
        fixes.Add(new KeyValuePair<string, int>(word, to));
        //StartCoroutine(CoUtils.RunAfterDelay(.3f, () => {
        //    AudioManager.Instance.PlaySFX("intertitle_highlight");
        //}));
    }

    private void AppendGarbage(StringBuilder sb, int count, int at) {
        var garbage = GarbageTextGenerator.Generate(count);
        for (var i = 0; i < count; i += 1) {
            if (fader >= timings[at + i]) {
                sb.Append(garbage[i]);
            } else {
                sb.Append(' ');
            }
        }
    }
}
