﻿public static class ContinueSwitches {

    private static readonly string[] switchesD1 = { };

    private static readonly string[] switchesN1 = {
        "day1_01_gray",
        "day1_02_intro_lia",
        "day1_03_intro_cal",
        "day1_04_intro_owen",
        "day1_05_intro_nadine",
        "day1_06_intro_joey",
        "day1_intro",
        "day1_07_cal",
        "day1_08_meeting",
        "day1_09_search_joey",
        "day1_10_search_nadine",
        "day1_11_search_owen",
        "day1_12_search_gray",
        "day1_13_search_connie",
        "d1_clear",
    };

    private static readonly string[][] checkpoints = { switchesD1, switchesN1 };

    public static void Activate(int checkpoint) {
        for (var i = 0; i < checkpoint; i += 1 ) {
            var switches = checkpoints[i];
            foreach (var @switch in switches) {
                Global.Instance.Data.SetSwitch(@switch, true);
            }
        }
        if (checkpoint % 2 == 1) {
            Global.Instance.Data.SetSwitch("night", true);
        }
    }
}