using UnityEngine;
using UnityEngine.UI;

public class DeductionButton : MonoBehaviour {

    public Text header;
    public string desc;
    public Button button;
    public string command;
    public string @switch;
    public Image icon;

    public void Update() {
        if (!string.IsNullOrEmpty(@switch)) {
            button.interactable = Global.Instance.Data.GetSwitch(@switch);
            icon.enabled = Global.Instance.Data.GetSwitch(@switch);
        }
    }

    public void OnMouseOver() {
        if (icon.enabled) {
            header.text = desc;
        }
    }

    public void OnMouseExit() {
        if (header.text == desc) {
            header.text = null;
        }
    }
}
