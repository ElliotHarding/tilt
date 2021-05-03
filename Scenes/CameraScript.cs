using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject m_ball;
    public float m_zPosition = -1;

    void Update()
    {
        transform.position = new Vector3(0, m_ball.transform.position.y + 2, m_zPosition);
    }
}
