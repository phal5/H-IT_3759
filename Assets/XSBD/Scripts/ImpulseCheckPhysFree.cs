using UnityEngine;

public class ImpulseCheckPhysFree : MonoBehaviour
{
    [SerializeField] float _mass;
    [SerializeField] float _threshold = 0;
    [SerializeField] Vector3 _impulse;
    [Space(10f)]
    [SerializeField] Vector3 _acceleration = Vector3.zero;
    [SerializeField] Vector3 _bufferedAcceleration = Vector3.zero;
    [SerializeField]public Vector3 _bufferedImpulse { get; private set; } = Vector3.zero;
    [Space(30f)]
    [SerializeField] uint _accelerationBufferSize = 10;
    [SerializeField] bool _accurateSum = false;

    Vector3[] _accelerationBuffer;
    Vector3 _velocity = Vector3.zero;
    Vector3 _prevVelocity = Vector3.zero;
    Vector3 _prevPos;
    Vector3 _sumOfAccelerationBuffer = Vector3.zero;
    float _invAccelerationBufferSize;
    float _invDeltaTime;
    float _invG = 1f / 9.81f;
    uint _accelerationBufferIndex;

    // Start is called before the first frame update
    void Start()
    {
        _prevPos = transform.position;
        _prevVelocity = _velocity;
        _accelerationBuffer = new Vector3[_accelerationBufferSize];
        _invAccelerationBufferSize = 1f / _accelerationBufferSize;
        _invDeltaTime = 1f / Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        VelocityUpdate();
        AccelerationUpdate();
        if(_accurateSum) { GetNewSumAccurate(out _sumOfAccelerationBuffer); }
        else { GetNewSum(ref _sumOfAccelerationBuffer); }

        _bufferedAcceleration = _sumOfAccelerationBuffer * _invAccelerationBufferSize;
        _bufferedImpulse = _bufferedAcceleration * _mass * _invG;

        if (Vector3.SqrMagnitude(_bufferedImpulse) > _threshold * _threshold) Debug.Log(_impulse = _bufferedImpulse);
    }

    void VelocityUpdate()
    {
        _velocity = (transform.position - _prevPos) * _invDeltaTime;
        _prevPos = transform.position;
    }

    void AccelerationUpdate()
    {
        //_prevVelocity holds the velocity of the previous frame
        _acceleration = (_velocity - _prevVelocity) / Time.fixedDeltaTime;
        //save current velocity to _prevVelocity to use in next frame
        _prevVelocity = _velocity;
    }

    void GetNewSum(ref Vector3 sum)
    {
        //increment _accelerationBufferIndex by 1 and set to 0 when it hits the ceiling.
        if (++_accelerationBufferIndex == _accelerationBufferSize) _accelerationBufferIndex = 0;
        sum -= _accelerationBuffer[_accelerationBufferIndex] - _acceleration;
        _accelerationBuffer[_accelerationBufferIndex] = _acceleration;
    }

    void GetNewSumAccurate(out Vector3 sum)
    {
        //increment _accelerationBufferIndex by 1 and set to 0 when it hits the ceiling.
        if (++_accelerationBufferIndex == _accelerationBufferSize) _accelerationBufferIndex = 0;
        _accelerationBuffer[_accelerationBufferIndex] = _acceleration;

        //update sum
        sum = Vector3.zero;
        foreach(Vector3 vector in _accelerationBuffer)
        {
            sum += vector;
        }
    }
}
