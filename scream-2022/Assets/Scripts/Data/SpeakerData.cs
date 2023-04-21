using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeakerData", menuName = "Data/Speaker")]
public class SpeakerData : MainSchema {

    public string tag;
    public string displayName;
    public Sprite image;
    public List<Expression> expressions;
    public List<Sprite> glitchBrush;
    public Sprite glow;
    public Sprite spooky;
    public Sprite glitch;

    public override string Key => tag;

    public Sprite GetExpr(string tag) {
        foreach (var e in expressions) {
            if (e.tag == tag) {
                return e.sprite;
            }
        }
        return image;
    }
}

[System.Serializable]
public class Expression {

    public Sprite sprite;
    public string tag;
}
