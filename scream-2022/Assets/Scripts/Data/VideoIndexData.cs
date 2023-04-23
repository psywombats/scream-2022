using UnityEngine;
using System;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "VideoDataIndex", menuName = "Data/Index/Video")]
public class VideoIndexData : GenericIndex<VideoData> {

}

[Serializable]
public class VideoData : GenericDataObject {

    public enum Type {
        Good, Evil, Ad, None,
    }

    public int index;
    public Type type;
    public Color color;
}
