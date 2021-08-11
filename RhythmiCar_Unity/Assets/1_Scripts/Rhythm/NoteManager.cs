using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    private static NoteManager instance = null;
    public int bpm;
    double currentTime = 0d;
    [SerializeField] Transform LeftStartPosition;
    [SerializeField] Transform RightStratPosition;
    public bool isStart { get; set; }
    public AudioSource music;

    private void Awake()
    {

        if(instance ==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static NoteManager Instance
    {
        get
        {

            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime>=60d/bpm)
        {

            GameObject l_note = ObjectPooler.instance.LeftNoteQueue.Dequeue();
            GameObject r_note = ObjectPooler.instance.RightNoteQueue.Dequeue();
            l_note.transform.position = LeftStartPosition.position;
            r_note.transform.position = RightStratPosition.position;
            l_note.SetActive(true);
            r_note.SetActive(true);
            currentTime -= 60d / bpm;
        }
    }
}
