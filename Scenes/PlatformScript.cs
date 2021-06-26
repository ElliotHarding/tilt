using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public GameObject m_leftMover;
    public GameObject m_rightMover;
    public float m_moverSpeed = 10;

    //wall spawning stuff
    public GameObject m_rightWall;
    public GameObject m_leftWall;
    public float m_rightWallPos = 7;
    public float m_leftWallPos = -7;
    public const int m_cSpawnBufferHeight = 10;
    private float m_wallSpawnHeight = -10;
    private float m_bottomWallSpawnHeight = 10;
    private List<GameObject> m_walls = new List<GameObject>();

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

            setPlatformPositionAndRotation();
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

            setPlatformPositionAndRotation();
        }

        //Check if need to spawn new set of walls
        if(transform.position.y + m_cSpawnBufferHeight > m_wallSpawnHeight)
        {
            GameObject newRightWall = Instantiate(m_rightWall, new Vector3(m_rightWallPos, m_wallSpawnHeight, 0), m_rightWall.transform.rotation);
            GameObject newLeftWall = Instantiate(m_leftWall, new Vector3(m_leftWallPos, m_wallSpawnHeight, 0), m_leftWall.transform.rotation);

            m_walls.Add(newRightWall);
            m_walls.Add(newLeftWall);

            m_wallSpawnHeight += m_cSpawnBufferHeight;
        }

        if(transform.position.y - m_cSpawnBufferHeight < m_bottomWallSpawnHeight)
        {
            GameObject newRightWall = Instantiate(m_rightWall, new Vector3(m_rightWallPos, m_bottomWallSpawnHeight, 0), m_rightWall.transform.rotation);
            GameObject newLeftWall = Instantiate(m_leftWall, new Vector3(m_leftWallPos, m_bottomWallSpawnHeight, 0), m_leftWall.transform.rotation);

            m_walls.Add(newRightWall);
            m_walls.Add(newLeftWall);

            m_bottomWallSpawnHeight -= m_cSpawnBufferHeight;
        }

        //Check to delete old walls
        for(int i = 0; i < m_walls.Count; i++)
        {
            if (m_walls[i].transform.position.y < transform.position.y - m_cSpawnBufferHeight)
            {
                Destroy(m_walls[i]);
                m_walls.RemoveAt(i);
                m_bottomWallSpawnHeight += m_cSpawnBufferHeight / 2;
                continue;
            }

            if(m_walls[i].transform.position.y > transform.position.y + m_cSpawnBufferHeight)
            {
                Destroy(m_walls[i]);
                m_walls.RemoveAt(i);
                m_wallSpawnHeight -= m_cSpawnBufferHeight / 2;
                continue;
            }
        }
    }

    void setPlatformPositionAndRotation()
    {
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
