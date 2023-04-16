using UnityEngine;

public class FieldSpritesheetComponent : MonoBehaviour {

    [SerializeField] public SpritesheetData spritesheet;

    public int StepCount => spritesheet == null ? 1 : spritesheet.StepCount;
    public string Name => spritesheet == null ? "" : spritesheet.name;

    public Sprite GetFrame(OrthoDir dir, int step) {
        if (spritesheet == null) {
            return null;
        }
        if (spritesheet.IsSingleFrame) {
            step = 0;
        }
        if (spritesheet.IsSingleStrip) {
            dir = OrthoDir.North;
        }
        return spritesheet.GetSprite(dir, step);
    }

    public void SetByTag(string tag) {
        if (tag == null || tag == "") {
            // ?
        } else {
            spritesheet = IndexDatabase.Instance.FieldSprites.GetData(tag).sprite;
        }
    }

    public void Set(SpritesheetData data) {
        spritesheet = data;
    }

    public Sprite FrameForDirection(OrthoDir facing) {
        return GetFrame(facing, 0);
    }
}
