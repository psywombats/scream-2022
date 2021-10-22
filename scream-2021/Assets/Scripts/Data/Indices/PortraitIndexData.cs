using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PortraitIndexData", menuName = "Data/VN/PortraitIndex")]
public class PortraitIndexData : SerializableObjectIndex<PortraitData> {

}

[Serializable]
public class PortraitData : GenericDataObject {

    public Sprite portrait;
    public int offset;
    public string sfxName;
    public bool sudden;
}
