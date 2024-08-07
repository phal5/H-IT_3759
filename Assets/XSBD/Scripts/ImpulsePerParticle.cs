using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ImpulsePerParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem _system;
    [SerializeField] uint _bufferSize = 10;
    [SerializeField] float _particleMass = 1;
    
    ParticleSystem.Particle[] _particles;

    int _bufferIndex = 0;
    float _invBufferSize;
    float _invDeltaTime;
    float[] _lifetimes;
    Vector3[] _positions;
    [SerializeField] Vector3[] _velocities;
    Vector3[,] _accelerationBuffer;
    Vector3[] _sums;
    public Vector3[] _impulses { get; private set; }
    Transform _particleSystemTransform;
    // Start is called before the first frame update
    void Start()
    {
        _particleSystemTransform = _system.transform;
        _invBufferSize = 1.0f / Time.fixedDeltaTime;
        _invDeltaTime = 1.0f / Time.fixedDeltaTime;

        _particles = new ParticleSystem.Particle[_system.main.maxParticles];

        _lifetimes = new float[_system.main.maxParticles];
        _velocities = new Vector3[_system.main.maxParticles];
        _accelerationBuffer = new Vector3[_system.main.maxParticles, _bufferSize];
        _sums = new Vector3[_system.main.maxParticles];
        _impulses = new Vector3[_system.main.maxParticles];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _system.GetParticles(_particles);
        UpdateValues();
        CalculateImpulse();
    }

    void CalculateImpulse()
    {
        
        for(int i = 0; i < _impulses.Length; i++)
        {
            _impulses[i] = _sums[i] * _particleMass * _invBufferSize * Time.fixedDeltaTime;
        }
    }

    void UpdateValues()
    {
        for (int i = 0; i < _system.particleCount; ++i)
        {
            if (_particles[i].remainingLifetime > _lifetimes[i] || _particles[i].remainingLifetime == 0) ResetData(i);
            else
            {
                _sums[i] -= _accelerationBuffer[i, _bufferIndex];

                _lifetimes[i] = _particles[i].remainingLifetime;

                _accelerationBuffer[i, _bufferIndex] =
                    (_velocities[i] - (_velocities[i] = _particleSystemTransform.TransformDirection(_particles[i].velocity))) * _invDeltaTime;

                _sums[i] += _accelerationBuffer[i, _bufferIndex];
            }
        }
        if(++_bufferIndex == _bufferSize)
        {
            _bufferIndex = 0;
        }
    }

    void ResetData(int i)
    {
        _lifetimes[i] = _particles[i].remainingLifetime;
        _velocities[i] = _particleSystemTransform.TransformDirection(_particles[i].velocity);
        _sums[i] = Vector3.zero;
        _accelerationBuffer[i, _bufferIndex] = Vector3.zero;
    }
}
