using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviour
{
    [SerializeField] GameObject[] _these;
    [SerializeField] Vector3[] _thisFast;
    [Space(10f)]
    [SerializeField] bool _fire;

    Rigidbody[] _theseRigidbodies;
    Vector3[] _positions;

    // Start is called before the first frame update
    void Start()
    {
        _theseRigidbodies = new Rigidbody[_these.Length];
        _positions = new Vector3[_these.Length];
        for(int i = 0; i < _these.Length; i++)
        {
            _theseRigidbodies[i] = _these[i].GetComponent<Rigidbody>();
            _positions[i] = _these[i].transform.position;
        }
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(10, 10, 200, 50), "Fire"))
        {
            for (int i = 0; i < _these.Length; i++)
            {
                Launch(_theseRigidbodies[i], _positions[i], _thisFast[i]);
            }
        }
    }

    void Launch(Rigidbody rb, Vector3 position, Vector3 direction)
    {
        rb.gameObject.transform.position = position;
        rb.velocity = direction;
    }
}
