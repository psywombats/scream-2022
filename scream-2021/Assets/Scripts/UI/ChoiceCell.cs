using TMPro;
using UnityEngine;

public class ChoiceCell : MonoBehaviour {

    [SerializeField] public TextMeshProUGUI meshA = null;
    [SerializeField] public TextMeshProUGUI meshB = null;

    public void Populate(string text) {
        meshA.text = text;
        meshB.text = text;
    }
}