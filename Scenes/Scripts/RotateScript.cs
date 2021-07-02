using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public Vector3 m_rotationDirection;
    public float smoothTime;
    private float smooth;

    // Update is called once per frame
    void Update()
    {
        smooth = Time.deltaTime * smoothTime;
        transform.Rotate(m_rotationDirection * smooth);
    }
}
