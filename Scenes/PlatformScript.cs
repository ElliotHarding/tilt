using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlatformScript : MonoBehaviour
{
    //Platform movement
    public GameObject m_leftMover;
    public GameObject m_rightMover;
    public float m_moverSpeed = 10;

    //Checking if mouse is clicking on UI
    public EventSystem m_eventSystem;
    public GraphicRaycaster m_raycaster;

    //wall spawning stuff
    public GameObject m_rightWall;
    public GameObject m_leftWall;
    private float m_distToWall; //The distance to screen walls in world coords
    public float m_wallHeight = 10;
    public float m_wallWidth = 2;
    private float m_wallSpawnYPosition = 0;
    private const int m_cWallsSize = 3;
    private int m_bottomWallsIndex = 0;
    private int m_topWallsIndex = m_cWallsSize - 1;
    private List<Tuple<GameObject, GameObject>> m_walls = new List<Tuple<GameObject, GameObject>>();

    //Enemies spawning stuff
    public GameObject m_spikeBallPrefab;
    public GameObject m_leftGunPrefab;
    public GameObject m_rightGunPrefab;
    public GameObject m_spikeCrossPrefab;
    public GameObject m_movingSpikeBall;
    private int m_previousSpawnPrefabIndex = 0;
    private float m_enemySpawnYPosition = 0;
    private const int m_cEnemySpawnGap = 5;
    private List<GameObject> m_enemies = new List<GameObject>();

    void Start()
    {
        Vector3 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        m_distToWall = screenSize.x - m_wallWidth;
        
        GameObject newLeftWall = Instantiate(m_leftWall, new Vector3(-m_distToWall, -m_wallHeight, 0), m_leftWall.transform.rotation);
        GameObject newRightWall = Instantiate(m_rightWall, new Vector3(m_distToWall, -m_wallHeight, 0), m_rightWall.transform.rotation);

        GameObject newLeftWall1 = Instantiate(m_leftWall, new Vector3(-m_distToWall, 0, 0), m_leftWall.transform.rotation);
        GameObject newRightWall1 = Instantiate(m_rightWall, new Vector3(m_distToWall, 0, 0), m_rightWall.transform.rotation);

        GameObject newLeftWall2 = Instantiate(m_leftWall, new Vector3(-m_distToWall, m_wallHeight, 0), m_leftWall.transform.rotation);
        GameObject newRightWall2 = Instantiate(m_rightWall, new Vector3(m_distToWall, m_wallHeight, 0), m_rightWall.transform.rotation);
        
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
                //Make sure isnt clicking on UI
                if (!mouseOnUI(new Vector3(touch.position.x, touch.position.y)))
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

            setPlatformPositionAndRotation();
        }

        //Handle mouse clicks
        if (Input.GetMouseButton(0))
        {
            //Make sure mouse isnt clicking on UI
            if (!mouseOnUI(Input.mousePosition))
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

        //Check if we need to spawn enemy above
        if (transform.position.y - m_enemySpawnYPosition > m_cEnemySpawnGap)
        {
            m_enemySpawnYPosition = transform.position.y;
            spawnEnemy(true);

            //Check if theres any enemies far away to be deleted
            for (int i = 0; i < m_enemies.Count; i++)
            {
                if (m_enemies[i].transform.position.y < transform.position.y - m_cEnemySpawnGap * 4)
                {
                    Destroy(m_enemies[i]);
                    m_enemies.RemoveAt(i);
                }
            }
        }

        //Check if we need to spawn enemy below
        if (m_enemySpawnYPosition - transform.position.y > m_cEnemySpawnGap)
        {
            m_enemySpawnYPosition = transform.position.y;
            spawnEnemy(false);

            //Check if theres any enemies far away to be deleted
            for (int i = 0; i < m_enemies.Count; i++)
            {
                if(m_enemies[i].transform.position.y > transform.position.y + m_cEnemySpawnGap*4)
                {
                    Destroy(m_enemies[i]);
                    m_enemies.RemoveAt(i);
                }
            }
        }
    }

    bool mouseOnUI(Vector3 mousePosition)
    {
        //Check were not clicking in ui area
        List<RaycastResult> results = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(m_eventSystem);
        pointerEventData.position = mousePosition;
        m_raycaster.Raycast(pointerEventData, results);

        return results.Count != 0;
    }

    void spawnEnemy(bool above)
    {
        //Randomly select enemy, position it randomly (unless gun)
        GameObject chosenEnemyType;
        Vector3 spawnLocation;
        float spawnLocationY = transform.position.y + m_cEnemySpawnGap * (above ? 2 : -2);

        //Choose a index for a prefab, cant be the same as the previous prefab spawned
        int randomIndex = UnityEngine.Random.Range(0, 5);
        while (randomIndex == m_previousSpawnPrefabIndex)
            randomIndex = UnityEngine.Random.Range(0, 5);

        m_previousSpawnPrefabIndex = randomIndex;

        const int enemyWidth = 1; //Stops clipping into wall when spawned
        if (randomIndex == 0)
        {
            chosenEnemyType = m_spikeBallPrefab;
            spawnLocation = new Vector3(UnityEngine.Random.Range(-m_distToWall + enemyWidth, m_distToWall - enemyWidth), spawnLocationY, 0);
        }
        else if(randomIndex == 1)
        {
            chosenEnemyType = m_rightGunPrefab;
            spawnLocation = new Vector3(-m_distToWall, spawnLocationY, 0);
        }
        else if(randomIndex == 2)
        {
            chosenEnemyType = m_leftGunPrefab;
            spawnLocation = new Vector3(m_distToWall, spawnLocationY, 0);
        }
        else if(randomIndex == 3)
        {
            chosenEnemyType = m_movingSpikeBall;
            spawnLocation = new Vector3(UnityEngine.Random.Range(-m_distToWall + enemyWidth, m_distToWall - enemyWidth), spawnLocationY, 0);
        }
        else
        {
            chosenEnemyType = m_spikeCrossPrefab;
            spawnLocation = new Vector3(UnityEngine.Random.Range(-m_distToWall + enemyWidth, m_distToWall - enemyWidth), spawnLocationY, 0);
        }

        GameObject newEnemy = Instantiate(chosenEnemyType, spawnLocation, m_spikeBallPrefab.transform.rotation);
        m_enemies.Add(newEnemy);
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

        /*
        float m = Mathf.Tan(rotation);
        float c = transform.position.y + 0.25f - m * transform.position.x;
        double dist = (Math.Abs(m * m_ball.transform.position.x + m_ball.transform.position.y + c)) / Math.Sqrt(m * m + 1);
        if (dist < m_ball.transform.localScale.y)
        {
            double correctionDistance = m_ball.transform.localScale.y - dist;
            m_ball.transform.position += new Vector3(0, (float)correctionDistance);
        } */
    }
}
