using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImpulseComponent : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidBody;
    [Space(10f)]
    [SerializeField] float _threshold = 0;
    [SerializeField] Vector3 _impulse;
    [Space(10f)]
    [SerializeField] Vector3 _acceleration = Vector3.zero;
    [SerializeField] Vector3 _bufferedAcceleration = Vector3.zero;
    public Vector3 _bufferedImpulse {  get; private set; } = Vector3.zero;
    [Space(30f)]
    [SerializeField] uint _accelerationBufferSize = 10;

    Vector3[] _accelerationBuffer;
    Vector3 _prevVelocity = Vector3.zero;
    float _invAccelerationBufferSize;
    float _invG = 1f / 9.81f;
    uint _accelerationBufferIndex;

    // Start is called before the first frame update
    void Start()
    {
        _accelerationBuffer = new Vector3[_accelerationBufferSize];
        _invAccelerationBufferSize = 1f / _accelerationBufferSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AccelerationUpdate();
        StoreToBuffer(_acceleration);
        _bufferedAcceleration = SumOfBuffer() * _invAccelerationBufferSize;
        _bufferedImpulse = (_bufferedAcceleration) * _rigidBody.mass * _invG;
        if (Vector3.SqrMagnitude(_bufferedImpulse) > _threshold * _threshold)
        {
            _impulse = _bufferedImpulse;
            Debug.Log(_impulse);
        }
        else _impulse = Vector3.zero;
    }

    void AccelerationUpdate()
    {
        //_prevVelocity holds the velocity of the previous frame
        _acceleration = (_rigidBody.velocity - _prevVelocity) / Time.fixedDeltaTime;
        //save current velocity to _prevVelocity to use in next frame
        _prevVelocity = _rigidBody.velocity;
    }

    void StoreToBuffer(Vector3 acceleration)
    {
        //increment _accelerationBufferIndex by 1 and set to 0 when it hits the ceiling.
        if(++_accelerationBufferIndex == _accelerationBufferSize) _accelerationBufferIndex = 0;
        _accelerationBuffer[_accelerationBufferIndex] = acceleration;
    }

    Vector3 SumOfBuffer()
    {
        Vector3 _accelerationBufferSum = Vector3.zero;
        foreach(Vector3 v in _accelerationBuffer) _accelerationBufferSum += v;
        return _accelerationBufferSum;
    }
}
