using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour {

    public Button button;
    public TitleController title;
    public int continueIndex;

    public void Start() {
        button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX("cursor");
            title.ResumeBookmark(continueIndex);
        });
    }
}