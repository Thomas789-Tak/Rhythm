using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbController : MonoBehaviour
{
    [SerializeField] Transform parent;
    void Start()
    {
        Invoke("SetProbToParent",1f);
    }

    void SetProbToParent()
    {

        transform.SetParent(parent);
    }
}
