﻿using DG.Tweening;
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
    public CanvasGroup background;

    public Image wake0, wake1, wake2;

    public bool IsShown { get; private set; }

    public Dictionary<SpeakerData, string> speakerNames = new Dictionary<SpeakerData, string>();

    public void Wipe() {
        text.Clear();
        nameText.text = "";
    }

    public IEnumerator ShowRoutine(bool lightMode = false) {
        IsShown = true;
        backer.Hide();
        fader.alpha = 0.0f;
        background.alpha = 0f;
        foreach (var portrait in GetPortraits()) {
            portrait.Clear();
        }

        if (!lightMode) {
            StartCoroutine(CoUtils.RunTween(background.DOFade(1, bgTime)));
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
        routines.Add(CoUtils.RunTween(background.DOFade(0, bgTime)));
        yield return CoUtils.RunParallel(routines.ToArray(), this);
        Wipe();
        IsShown = false;
    }

    public IEnumerator EnterRoutine(SpeakerData speaker, string slot, string expr = null) {
        var portrait = GetPortrait(slot);
        yield return portrait.EnterRoutine(speaker, expr);
    }

    public IEnumerator ExitRoutine(SpeakerData speaker) {
        foreach (var portrait in GetPortraits()) {
            if (portrait.Speaker == speaker) {
                yield return portrait.ExitRoutine();
            }
        }
    }

    public void SetWake(int count) {
        Image waker = null;
        if (count == 0) waker = wake0;
        if (count == 1) waker = wake1;
        if (count == 2) waker = wake2;

        foreach (var un in new List<Image>() { wake0, wake1, wake2 }) {
            if (waker == un) {
                un.DOFade(1f, 2f).Play();
            } else {
                un.DOFade(0f, 2f).Play();
            }
        }
    }

    public IEnumerator SpeakRoutine(SpeakerData speaker, string message) {
        Wipe();
        var name = speaker.displayName;
        
        if (!IsShown) {
            yield return ShowRoutine(lightMode: true);
        }

        if (speaker != null) {
            yield return SetHighlightRoutine(speaker);
        }

        string toType = message;
        nameText.text = name;
        yield return text.WriteLineRoutine(toType);
        yield return Global.Instance.Input.ConfirmRoutine();
    }

    public IEnumerator SetHighlightRoutine(SpeakerData speaker) {
        var portrait = GetPortrait(speaker);
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

    public PortraitComponent GetPortrait(SpeakerData speaker) {
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
