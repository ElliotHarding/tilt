using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHorizontaly : MonoBehaviour
{
    public float m_moveSpeed = 5f;

    private float m_distToWall;
    private const float m_cWallWidth = 3;
    private bool m_bGoingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        m_distToWall = screenSize.x - m_cWallWidth;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bGoingRight)
        {
            transform.position += new Vector3(m_moveSpeed * Time.deltaTime, 0);

            if (transform.position.x > m_distToWall)
            {
                m_bGoingRight = false;
            }
        }
        else
        {
            transform.position += new Vector3(-m_moveSpeed * Time.deltaTime, 0);

            if(transform.position.x < -m_distToWall)
            {
                m_bGoingRight = true;
            }
        }
    }
}
