using System.Collections;
using UnityEngine;

public class Slot<T> : MonoBehaviour
{
    protected T content;
    

    public virtual void SetContent(T newContent)
    {
        content = newContent;
    }

    public T GetContent()
    {
        return content;
    }

    public void SetPath(string path)
    {
        
    }
}