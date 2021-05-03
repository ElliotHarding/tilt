using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public GameObject m_leftMover;
    public GameObject m_rightMover;
    public float m_moverSpeed = 10;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

                if (touchPos.x > 0)
                {
                    float yDiff = touchPos.y - m_rightMover.transform.position.y;
                    m_rightMover.transform.position += new Vector3(0, yDiff * Time.deltaTime * m_moverSpeed, 0);
                }
                else
                {
                    float yDiff = touchPos.y - m_leftMover.transform.position.y;
                    m_leftMover.transform.position += new Vector3(0, yDiff * Time.deltaTime * m_moverSpeed, 0);
                }
            }
        }

        //Handle mouse clicks
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

            if (mousePos.x > 0)
            {
                float yDiff = mousePos.y - m_rightMover.transform.position.y;
                m_rightMover.transform.position += new Vector3(0, yDiff * Time.deltaTime * m_moverSpeed, 0);
            }
            else
            {
                float yDiff = mousePos.y - m_leftMover.transform.position.y;
                m_leftMover.transform.position += new Vector3(0, yDiff * Time.deltaTime * m_moverSpeed, 0);
            }
        }


        //Set platform position
        Vector3 midpoint = new Vector3(0, (m_rightMover.transform.position.y + m_leftMover.transform.position.y) / 2);
        transform.position = midpoint;

        //Set platform rotation
        Vector2 diference = m_rightMover.transform.position - m_leftMover.transform.position;
        float sign = (m_rightMover.transform.position.y < m_leftMover.transform.position.y) ? -1.0f : 1.0f;
        float rotation = Vector2.Angle(Vector2.right, diference) * sign;

        if (rotation > 15)
            rotation = 15;
        if (rotation < -15)
            rotation = -15;

        transform.localEulerAngles = new Vector3(0, 0, rotation);
    }
}
