using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedDTCheck : MonoBehaviour
{
    float _fdt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (_fdt != Time.fixedDeltaTime) Debug.LogError(_fdt = Time.fixedDeltaTime);
    }
}
