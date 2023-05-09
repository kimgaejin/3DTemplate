using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedFunction_update : MonoBehaviour
{
    private readonly int SIZE = 100000;
    public float[] _timers;

    private bool _isCount = true;
    private int _count = 0;

    public void Awake()
    {
        _timers = new float[SIZE];
    }

    public void Update()
    {
        var deltaTime = Time.deltaTime;
        for (int i = 0; i < SIZE; i++)
        {
            if (1.0f < _timers[i])
                continue;
            _timers[i] += deltaTime;
            if (1.0f <= _timers[i])
            {
                Something();
            }
        }
        if (_isCount)
            _count++;
        Debug.Log($"Update {Time.deltaTime} {_count}");
    }

    public void Something()
    {
        _isCount = false;
        int a = 3;
        int b = 5;
        int c = a * b;
        int d = c + a;
    }
}