using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

[JsonObject(MemberSerialization.OptIn)]
public class SaveInfoData {



    public SaveInfoData() {
        // serialized
    }

    public SaveInfoData(GameData data) {

    }
}
