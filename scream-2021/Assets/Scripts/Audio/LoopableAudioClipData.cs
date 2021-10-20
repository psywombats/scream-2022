using UnityEngine;

[CreateAssetMenu(fileName = "LoopableAudioClipData", menuName = "Data/LoopableAudioClipData")]
public class LoopableAudioClipData : ScriptableObject {

    public AudioClip clip;
    public long loopBeginSample = 0;
    public long loopEndSample;
    public bool loops = true;
}
