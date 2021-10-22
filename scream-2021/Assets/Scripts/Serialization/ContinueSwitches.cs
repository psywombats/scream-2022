public static class ContinueSwitches {

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

    private static readonly string[] switchesD2 = {
        "night1_00_lia",
        "night1_01_owen",
        "night1_lever",
        "night1_lever_2",
        "night1_04_cal",
        "n1_clear",
    };

    private static readonly string[] switchesN2 = {
        "day2_01_owen",
        "day2_02_cal",
        "day2_03_meeting",
        "day2_05_connie",
        "day2_06_gray",
        "day2_09_post_owen",
        "d2_clear",
    };

    private static readonly string[] switchesD3 = {
        "night2_01_owenroom",
        "night2_02_joey",
        "night2_04_paper1",
        "night2_06_cal",
        "night2_alert",
        "n2_clear",
    };

    private static readonly string[] switchesN3 = {
        "day3_00_lia",
        "day3_01_gray",
        "day3_02_nadine",
        "day3_03_joey",
        "day3_04_owen",
        "day3_intro",
        "d3_clear",
    };

    private static readonly string[] switchesD4 = {
        "night3_00_lia",
        "night3_connie",
    };

    private static readonly string[][] checkpoints = { switchesD1, switchesN1, switchesD2, switchesN2, switchesD3, switchesN3 };

    public static void Activate(int checkpoint) {
        for (var i = 0; i <= checkpoint; i += 1 ) {
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