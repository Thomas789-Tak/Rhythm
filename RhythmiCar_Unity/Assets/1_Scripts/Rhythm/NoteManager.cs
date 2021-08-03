using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    private static NoteManager instance = null;    
    public int bpm = 0;
    double currentTime = 0d;
    [SerializeField] GameObject LNote;
    [SerializeField] GameObject RNote;
    [SerializeField] Transform LeftStartPosition;
    [SerializeField] Transform RightStratPosition;
    bool isStart;
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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime>=60d/bpm)
        {
            GameObject L_ControllNote = Instantiate(LNote, LeftStartPosition.position, Quaternion.identity);
            GameObject R_ControllNote = Instantiate(RNote, RightStratPosition.position, Quaternion.identity);
            L_ControllNote.transform.SetParent(this.transform);
            R_ControllNote.transform.SetParent(this.transform);
            currentTime -= 60d / bpm;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            collision.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isStart==false)
        {
            if(collision.CompareTag("Note"))
            {
                music.Play();
                isStart = true;
            }
        }
    }
}
