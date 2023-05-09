using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedFunction_invoke : MonoBehaviour
{
    private readonly int SIZE = 100000;
    public float[] _timers;

    private bool _isCount = true;
    private int _count = 0;

    public void Awake()
    {
        for (int i = 0; i < SIZE; i++)
        {
            Invoke("Something", 1.0f);
        }
    }

    public void Update()
    {
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