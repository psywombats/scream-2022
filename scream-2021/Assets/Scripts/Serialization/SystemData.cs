using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class SystemData {

    public enum WindowMode {
        Fullscreen,
        Windowed,
    }

    [JsonProperty] public int LastSaveSlot { get; set; } = -1;
    [JsonProperty] public int LastTitle { get; set; } = 0;

    [JsonProperty] public SaveInfoData[] SaveInfo { get; private set; } = new SaveInfoData[SerializationManager.SaveSlotCount];

    [JsonProperty] public Setting<int> SettingWindow { get; private set; } =                    new Setting<int>((int)WindowMode.Windowed);
    [JsonProperty] public Setting<float> SettingTextSpeed { get; private set; } =               new Setting<float>(1.0f);
    [JsonProperty] public Setting<float> SettingMusicVolume { get; private set; } =             new Setting<float>(0.9f);
    [JsonProperty] public Setting<float> SettingSoundEffectVolume { get; private set; } =       new Setting<float>(0.9f);
    [JsonProperty] public Setting<float> SettingWalkSpeed { get; private set; } =               new Setting<float>(1.0f);
}
