using UnityEngine;

[CreateAssetMenu(fileName = "SpeakerData", menuName = "Data/Speaker")]
public class SpeakerData : MainSchema {

    public string tag;
    public string displayName;
    public Sprite image;
    public Sprite altimage;
    public Sprite glow;

    public override string Key => tag;
}
