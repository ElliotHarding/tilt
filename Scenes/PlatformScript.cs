using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformScript : MonoBehaviour
{
    //Platform movement
    public GameObject m_leftMover;
    public GameObject m_rightMover;
    public float m_moverSpeed = 10;

    //wall spawning stuff
    public GameObject m_rightWall;
    public GameObject m_leftWall;
    private float m_wallSpawnX;
    public float m_wallHeight = 10;
    public float m_wallWidth = 2;
    private float m_wallSpawnYPosition = 0;
    private const int m_cWallsSize = 3;
    private int m_bottomWallsIndex = 0;
    private int m_topWallsIndex = m_cWallsSize - 1;
    private List<Tuple<GameObject, GameObject>> m_walls = new List<Tuple<GameObject, GameObject>>();

    //Spike ball spawning stuff
    public GameObject m_spikeBallPrefab;

    void Start()
    {
        Vector3 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        m_wallSpawnX = screenSize.x - m_wallWidth;

        GameObject newLeftWall = Instantiate(m_leftWall, new Vector3(-m_wallSpawnX, -m_wallHeight, 0), m_leftWall.transform.rotation);
        GameObject newRightWall = Instantiate(m_rightWall, new Vector3(m_wallSpawnX, -m_wallHeight, 0), m_rightWall.transform.rotation);

        GameObject newLeftWall1 = Instantiate(m_leftWall, new Vector3(-m_wallSpawnX, 0, 0), m_leftWall.transform.rotation);
        GameObject newRightWall1 = Instantiate(m_rightWall, new Vector3(m_wallSpawnX, 0, 0), m_rightWall.transform.rotation);

        GameObject newLeftWall2 = Instantiate(m_leftWall, new Vector3(-m_wallSpawnX, m_wallHeight, 0), m_leftWall.transform.rotation);
        GameObject newRightWall2 = Instantiate(m_rightWall, new Vector3(m_wallSpawnX, m_wallHeight, 0), m_rightWall.transform.rotation);
        
        m_walls.Add(new Tuple<GameObject, GameObject>(newLeftWall, newRightWall));
        m_walls.Add(new Tuple<GameObject, GameObject>(newLeftWall1, newRightWall1));
        m_walls.Add(new Tuple<GameObject, GameObject>(newLeftWall2, newRightWall2));
    }

    void Update()
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

        //Check if were moving up and organise walls appropriatly
        if(transform.position.y - m_wallSpawnYPosition > m_wallHeight)
        {
            swapWallsUp();
            m_wallSpawnYPosition = transform.position.y;
        }

        //Check if were moving down and organise walls appropriatly
        if(m_wallSpawnYPosition - transform.position.y > m_wallHeight)
        {
            swapWallsDown();
            m_wallSpawnYPosition = transform.position.y;
        }
    }

    void swapWallsDown()
    {
        m_walls[m_topWallsIndex].Item1.transform.position = m_walls[m_bottomWallsIndex].Item1.transform.position + new Vector3(0, -m_wallHeight);
        m_walls[m_topWallsIndex].Item2.transform.position = m_walls[m_bottomWallsIndex].Item2.transform.position + new Vector3(0, -m_wallHeight);

        m_bottomWallsIndex = m_topWallsIndex;
        m_topWallsIndex--;

        if (m_topWallsIndex == -1)
            m_topWallsIndex = m_cWallsSize -1;
    }

    void swapWallsUp()
    {
        m_walls[m_bottomWallsIndex].Item1.transform.position = m_walls[m_topWallsIndex].Item1.transform.position + new Vector3(0, m_wallHeight);
        m_walls[m_bottomWallsIndex].Item2.transform.position = m_walls[m_topWallsIndex].Item2.transform.position + new Vector3(0, m_wallHeight);

        m_topWallsIndex = m_bottomWallsIndex;
        m_bottomWallsIndex++;

        if(m_bottomWallsIndex == m_cWallsSize)
            m_bottomWallsIndex = 0;
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
