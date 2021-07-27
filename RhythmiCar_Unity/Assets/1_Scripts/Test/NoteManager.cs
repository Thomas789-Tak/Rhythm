using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    [SerializeField] GameObject LNote;
    [SerializeField] GameObject RNote;
    [SerializeField] Transform LPos;
    [SerializeField] Transform RPos;
    bool isStart;
    public AudioSource music;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime>=60d/bpm)
        {
            GameObject L_ControllNote = Instantiate(LNote, LPos.position, Quaternion.identity);
            GameObject R_ControllNote = Instantiate(RNote, RPos.position, Quaternion.identity);
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
