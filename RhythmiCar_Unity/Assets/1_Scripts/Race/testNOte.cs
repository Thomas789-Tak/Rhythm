using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testNOte : MonoBehaviour
{
    Image myImage;
    public Image aa;
    public Image aB;
    public Image aC;
    float pos;
    void Start()
    {
        myImage = GetComponent<Image>();
    }

    void Update()
    {
        pos += Time.deltaTime * 1f;
        myImage.rectTransform.position = new Vector2(myImage.rectTransform.position.x, myImage.rectTransform.position.y - pos);
        myImage.rectTransform.localScale = new Vector3(myImage.rectTransform.localScale.x+pos*0.002f, myImage.rectTransform.localScale.y, myImage.rectTransform.localScale.z);
        Invoke("ba", 2f);
        Invoke("bb", 4f);
        Invoke("bc", 6f);
    }

}
