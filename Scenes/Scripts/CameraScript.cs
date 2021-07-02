using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject m_ball;
    public float m_zPosition = -1;
    public float m_moveSpeed = 1;

    void Start()
    {
        transform.position = new Vector3(0, m_ball.transform.position.y + 1, m_zPosition);
    }

    void Update()
    {
        float yDiff = m_ball.transform.position.y - transform.position.y + 1;
        transform.position += new Vector3(0, yDiff * Time.deltaTime * m_moveSpeed);
    }
}
