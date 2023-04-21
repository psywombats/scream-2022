public static class ContinueSwitches {

    private static readonly string[] switchesPt1A = {
        "corridor01",
        "pt1_02",
        "pt1_03",
        "pt1_04",
        "sumi_last_elevator",
    };

    private static readonly string[] switchesPt1B = {
        "pt1_5a",
        "pt1_5b",
        "pt1_5c",
        "pt1_5d",
        "pt1_5e",
        "pt1_06",
        "pt1_08",
    };

    private static readonly string[] switchesPt1C = {
    };


    private static readonly string[][] checkpoints = { switchesPt1A, switchesPt1B, switchesPt1C };

    public static void Activate(int checkpoint) {
        for (var i = 0; i <= checkpoint; i += 1 ) {
            var switches = checkpoints[i];
            foreach (var @switch in switches) {
                Global.Instance.Data.SetSwitch(@switch, true);
            }
        }
    }
}