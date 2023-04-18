using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NVLComponent : MonoBehaviour {

    private static float bgTime = 0.5f;

    public PortraitComponent slotA;
    public PortraitComponent slotB;
    public PortraitComponent slotC;
    public PortraitComponent slotD;
    public PortraitComponent slotE;

    public ExpanderComponent backer;
    public LineAutotyper text;
    public Text nameText;
    public CanvasGroup fader;

    public Dictionary<SpeakerData, string> speakerNames = new Dictionary<SpeakerData, string>();

    public void Wipe() {
        text.Clear();
        nameText.text = "";
    }

    public IEnumerator ShowRoutine(bool dontClear = false) {
        backer.Hide();
        fader.alpha = 0.0f;
        if (!dontClear)
        {
            foreach (var portrait in GetPortraits())
            {
                portrait.Clear();
            }
        }
        
        yield return backer.ShowRoutine();
        text.Clear();
        Wipe();
    }

    public IEnumerator HideRoutine() {
        var routines = new List<IEnumerator>();
        foreach (var portrait in GetPortraits()) {
            if (portrait.Speaker != null) {
                routines.Add(portrait.ExitRoutine());
            }
        }
        yield return CoUtils.RunParallel(routines.ToArray(), this);
        routines.Clear();
        routines.Add(backer.HideRoutine());
        routines.Add(CoUtils.RunTween(fader.DOFade(0.0f, backer.duration)));
        yield return CoUtils.RunParallel(routines.ToArray(), this);
        Wipe();
    }

    public IEnumerator EnterRoutine(SpeakerData speaker, string slot, bool alt = false) {
        var portrait = GetPortrait(slot);
        yield return portrait.EnterRoutine(speaker, alt);
    }

    public IEnumerator ExitRoutine(SpeakerData speaker) {
        foreach (var portrait in GetPortraits()) {
            if (portrait.Speaker == speaker) {
                yield return portrait.ExitRoutine();
            }
        }
    }

    public IEnumerator SpeakRoutine(SpeakerData speaker, string message) {
        Wipe();
        var name = speakerNames.ContainsKey(speaker) ? speakerNames[speaker] : "????";

        var portrait = GetPortrait(speaker);
        if (speaker != null) {
            var routines = new List<IEnumerator>();
            if (portrait != null && !portrait.IsHighlighted) {
                routines.Add(portrait.HighlightRoutine());
            }
            foreach (var other in GetPortraits()) {
                if (other.Speaker != null && other.Speaker != speaker && other.IsHighlighted) {
                    routines.Add(other.UnhighlightRoutine());
                }
            }
            yield return CoUtils.RunParallel(routines.ToArray(), this);
        }

        string toType = message;
        nameText.text = name;
        yield return text.WriteLineRoutine(toType);
        yield return Global.Instance.Input.ConfirmRoutine();
    }

    private PortraitComponent GetPortrait(string slot) {
        PortraitComponent portrait = null;
        switch (slot.ToLower()) {
            case "a": portrait = slotA; break;
            case "b": portrait = slotB; break;
            case "c": portrait = slotC; break;
            case "d": portrait = slotD; break;
            case "e": portrait = slotE; break;
        }
        return portrait;
    }

    private PortraitComponent GetPortrait(SpeakerData speaker) {
        if (slotA.Speaker == speaker) return slotA;
        if (slotB.Speaker == speaker) return slotB;
        if (slotC.Speaker == speaker) return slotC;
        if (slotD.Speaker == speaker) return slotD;
        if (slotE.Speaker == speaker) return slotE;
        return null;
    }

    private List<PortraitComponent> GetPortraits() {
        return new List<PortraitComponent>() {
            slotA,
            slotB,
            slotC,
            slotD,
            slotE,
        };
    }
}
