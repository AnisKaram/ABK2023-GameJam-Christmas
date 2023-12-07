using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATMOX_DayNightCycle : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.5f;

    void FixedUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
        //transform.localEulerAngles.x + ((rotationSpeed / 10) * Time.deltaTime)
        transform.localEulerAngles = new Vector3(Time.time * rotationSpeed, 20, 0);
    }
}
