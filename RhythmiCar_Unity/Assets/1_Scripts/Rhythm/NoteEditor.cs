using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEditor : MonoBehaviour
{
    private bool isCheck = false;

    public void Check()
    {
        this.isCheck = !isCheck;

        var scale = this.transform.localScale;
        scale.x = isCheck ? 3f : 10f;
        this.transform.localScale = scale;
    }

    public bool IsChecked()
    {
        return isCheck;
    }
}
