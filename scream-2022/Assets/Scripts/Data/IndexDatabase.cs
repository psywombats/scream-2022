using UnityEngine;

[CreateAssetMenu(fileName = "IndexDatabase", menuName = "Data/IndexDatabase")]
public class IndexDatabase : ScriptableObject {

    public TransitionIndexData Transitions;
    public FadeIndexData Fades;
    public SoundEffectIndexData SFX;
    public BGMIndexData BGM;
    public SpeakerIndexData Speakers;
    public VideoIndexData Videos;

    public static IndexDatabase Instance => Resources.Load<IndexDatabase>("Database/Database");
}
