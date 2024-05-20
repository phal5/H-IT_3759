using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqrtIterator : MonoBehaviour
{
    [SerializeField] int iteration = 100000000;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < iteration; i++)
        {
            Mathf.Sqrt(Random.value);
        }
    }
}
