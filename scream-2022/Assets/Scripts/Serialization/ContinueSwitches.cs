public static class ContinueSwitches {

    private static readonly string[] switchesPt1A = { "corridor01", "pt1_02" };
    

    private static readonly string[][] checkpoints = { switchesPt1A };

    public static void Activate(int checkpoint) {
        for (var i = 0; i <= checkpoint; i += 1 ) {
            var switches = checkpoints[i];
            foreach (var @switch in switches) {
                Global.Instance.Data.SetSwitch(@switch, true);
            }
        }
    }
}