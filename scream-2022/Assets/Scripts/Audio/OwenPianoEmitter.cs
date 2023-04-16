using FMODUnity;
using UnityEngine;

public class OwenPianoEmitter : MonoBehaviour {
    public StudioEventEmitter sfx;

    public void Startle() {
        sfx.SetParameter("Piano_Transition", 1);
    }
}