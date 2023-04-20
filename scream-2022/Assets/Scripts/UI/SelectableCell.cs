using System;
using UnityEngine;

public class SelectableCell : MonoBehaviour {

    public GameObject selectedState = null;

    public event Action<SelectableCell, bool> onSelectionChange;

    protected bool selectable = true;

    public virtual void SetSelected(bool selected) {
        selectedState?.SetActive(selected);
        onSelectionChange?.Invoke(this, selected);
    }

    public virtual void SetSelectable(bool selectable) {
        this.selectable = selectable;
    }

    public bool IsSelectable() {
        return gameObject.activeInHierarchy && selectable;
    }
}
