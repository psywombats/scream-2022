using UnityEngine;

[CreateAssetMenu(fileName = "IndexDatabase", menuName = "Data/IndexDatabase")]
public class IndexDatabase : ScriptableObject {

    private static IndexDatabase instance;
    public static IndexDatabase Instance => instance ?? (instance = LoadInstance());

    public TransitionIndexData Transitions;
    public FadeIndexData Fades;
    public SoundEffectIndexData SFX;
    public BGMIndexData BGM;
    public FieldSpriteIndexData FieldSprites;

    public static IndexDatabase LoadInstance() {
        return Resources.Load<IndexDatabase>("Database/Database");
    }
}
