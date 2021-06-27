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
    public float m_wallHeight = 10;
    private const int m_cWallsSize = 3;
    private int m_leftWallIndex = 0;
    private int m_rightWallIndex = m_cWallsSize - 1;
    private List<GameObject> m_leftWalls = new List<GameObject>();
    private List<GameObject> m_rightWalls = new List<GameObject>();

    void Start()
    {
        GameObject newRightWall = Instantiate(m_rightWall, new Vector3(m_rightWallPos, -m_wallHeight, 0), m_rightWall.transform.rotation);
        GameObject newLeftWall = Instantiate(m_leftWall, new Vector3(m_leftWallPos, -m_wallHeight, 0), m_leftWall.transform.rotation);

        GameObject newRightWall1 = Instantiate(m_rightWall, new Vector3(m_rightWallPos, 0, 0), m_rightWall.transform.rotation);
        GameObject newLeftWall1 = Instantiate(m_leftWall, new Vector3(m_leftWallPos, 0, 0), m_leftWall.transform.rotation);

        GameObject newRightWall2 = Instantiate(m_rightWall, new Vector3(m_rightWallPos, m_wallHeight, 0), m_rightWall.transform.rotation);
        GameObject newLeftWall2 = Instantiate(m_leftWall, new Vector3(m_leftWallPos, m_wallHeight, 0), m_leftWall.transform.rotation);

        m_rightWalls.Add(newRightWall);
        m_leftWalls.Add(newLeftWall);
        m_rightWalls.Add(newRightWall1);
        m_leftWalls.Add(newLeftWall1);
        m_rightWalls.Add(newRightWall2);
        m_leftWalls.Add(newLeftWall2);

    }

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

        if(Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("space pressed");
            swapWallsUp();
        }
    }

    void swapWallsDown()
    {

    }

    void swapWallsUp()
    {
        m_rightWalls[m_leftWallIndex].transform.position = m_rightWalls[m_rightWallIndex].transform.position + new Vector3(0, m_wallHeight);
        m_leftWalls[m_leftWallIndex].transform.position = m_leftWalls[m_rightWallIndex].transform.position + new Vector3(0, m_wallHeight);

        m_rightWallIndex = m_leftWallIndex;
        m_leftWallIndex++;

        if(m_leftWallIndex == m_cWallsSize)
            m_leftWallIndex = 0;
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
