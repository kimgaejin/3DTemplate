using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAutoMovement : MonoBehaviour
{
    private float _timer = 0;
    private Vector3 _speed;
    private readonly float SIZE = 10.0f;

    private void Update()
    {
        if (_timer <= 0)
        {
            _speed = new Vector3(Random.Range(0.0f, 2.0f) - 1.0f, 0, Random.Range(0.0f, 2.0f) - 1.0f).normalized;
            transform.LookAt(transform.position + _speed);
            _timer = 3;
        }
        else
        {
            this.transform.position += _speed * Time.deltaTime;
        }

        _timer -= Time.deltaTime;
        if (transform.position.x < -SIZE)
            transform.position = Vector3.zero;
        if (transform.position.x > SIZE)
            transform.position = Vector3.zero;
        if (transform.position.z < -SIZE)
            transform.position = Vector3.zero;
        if (transform.position.z > SIZE)
            transform.position = Vector3.zero;
    }
}
