public static class ContinueSwitches {

    private static readonly string[] switchesPt1A = {
        "corridor01",
        "pt1_02",
        "pt1_03",
        "pt1_04",
        "sumi_last_elevator",
    };

    private static readonly string[] switchesPt1B = {
        "pt1_05a",
        "pt1_05b",
        "pt1_05c",
        "pt1_05d",
        "pt1_05e",
        "pt1_06",
        "pt1_08",
        "pt1_08b",
    };

    private static readonly string[] switchesPt1C = {
        "pt1_09a",
        "pt1_09b",
        "pt1_09c",
        "pt1_09d",
        "pt1_10",
    };

    private static readonly string[] switchesPt1D = {
        "pt1_10a",
        "pt1_10b",
        "pt1_10c",
        "pt1_11",
    };


    private static readonly string[][] checkpoints = { switchesPt1A, switchesPt1B, switchesPt1C, switchesPt1D };

    public static void Activate(int checkpoint) {
        for (var i = 0; i <= checkpoint; i += 1 ) {
            var switches = checkpoints[i];
            foreach (var @switch in switches) {
                Global.Instance.Data.SetSwitch(@switch, true);
            }
        }
    }
}