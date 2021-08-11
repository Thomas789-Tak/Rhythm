using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    public GameObject LNote;
    public GameObject RNote;
    public Transform tf_parent;
    public int count;
}
public class ObjectPooler : MonoBehaviour
{
    [SerializeField] ObjectInfo[] objectinfo = null;
    public static ObjectPooler instance;
    public Queue<GameObject> RightNoteQueue = new Queue<GameObject>();
    public Queue<GameObject> LeftNoteQueue = new Queue<GameObject>();
    void Start()
    {
        instance = this;
        LeftNoteQueue = InsertLeftQueue(objectinfo[0]);
        RightNoteQueue = InsertRightQueue(objectinfo[0]);
    }

    Queue<GameObject> InsertRightQueue(ObjectInfo p_objectInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();
        for(int i=0 ; i<p_objectInfo.count ; i++)
        {
            GameObject t_clone = Instantiate(p_objectInfo.RNote, transform.position, Quaternion.identity);
            t_clone.SetActive(false);
            if(p_objectInfo.tf_parent!=null)
            {
                t_clone.transform.SetParent(p_objectInfo.tf_parent);
            }
            else
            {
                t_clone.transform.SetParent(this.transform);
            }
            t_queue.Enqueue(t_clone);
        }
        return t_queue;
    }
    Queue<GameObject> InsertLeftQueue(ObjectInfo p_objectInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();
        for (int i = 0; i < p_objectInfo.count; i++)
        {
            GameObject t_clone = Instantiate(p_objectInfo.LNote, transform.position, Quaternion.identity);
            t_clone.SetActive(false);
            if (p_objectInfo.tf_parent != null)
            {
                t_clone.transform.SetParent(p_objectInfo.tf_parent);
            }
            else
            {
                t_clone.transform.SetParent(this.transform);
            }
            t_queue.Enqueue(t_clone);
        }
        return t_queue;
    }
}
