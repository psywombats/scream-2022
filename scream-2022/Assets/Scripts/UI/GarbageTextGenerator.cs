using UnityEngine;

public static class GarbageTextGenerator {

    private static string[] g1, g2, g3, g4, g5, g6;

    private static string g1s = "a a a b c f g h i i i j k m p q u v x x y z";
    private static string g2s = "of to in it is be as at so we he by or on do if me my up an go no us am ad";
    private static string g3s = "the and for are but not you all any can had her was one our out day get has him his how man new now old see two way who boy did its let put say she too use " +
        "hex sex ass lab red son god god god god god god dog bug cat sin sin sin dim pus via art con ere mad";
    private static string g4s = "that with have this will your from they know want been good much some time " +
        "baal ogre life lest unto amen pest grub worm vile mind surf mage cast redo dead rasp head dear ooze exit cage star ague bane dark girl damn hell lamb " +
        "wind vale live lash";
    private static string g5s = "blood death birth death devil demon video trans assay aught woman surge white black freak cawed hence heark angel leech";
    private static string g6s = "priest escape thrash strike struck hither judges indite invest lucife morrow physic pistol warden queens riband winter " +
        "throat cancer tongue sinner";

    private static string[][] gs;
    private static bool initd = false;

    public static string Generate(int length) {
        CheckInit();
        if (length < 0) {
            throw new System.ArgumentException();
        } else if (length == 0)  {
            return "";
        } else if (length <= 6) { 
            return RandGrab(length);
        } else {
            var split = Random.Range(1, length - 1);
            return Generate(split) + Generate(length - split);
        }
    }

    private static string RandGrab(int length) {
        var g = gs[length - 1];
        return g[Random.Range(0, g.Length)];
    }

    private static void CheckInit() {
        if (!initd) {
            initd = true;
            g1 = g1s.Split();
            g2 = g2s.Split();
            g3 = g3s.Split();
            g4 = g4s.Split();
            g5 = g5s.Split();
            g6 = g6s.Split();
        }
        gs = new string[][] { g1, g2, g3, g4, g5, g6 };
    }
}