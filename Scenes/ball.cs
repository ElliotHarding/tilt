using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ball : MonoBehaviour
{
    public GameObject m_gameOverPanel;

    //Score UI
    public Text m_scoreText;
    private int m_score = 0;
    private int m_highScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_highScore = PlayerPrefs.GetInt("highscore");
        m_scoreText.text = "Score: 0    High Score: " + m_highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Check and set score
        if (transform.position.y > m_score)
        {
            m_score = (int)transform.position.y;

            if (m_score > m_highScore)
            {
                m_highScore = m_score;
            }

            m_scoreText.text = "Score: " + m_score + "    High Score: " + m_highScore.ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name.Contains("wall") || col.gameObject.name.Contains("Spike") || col.gameObject.name.Contains("Beam"))
        {
            if(PlayerPrefs.GetInt("highscore") < m_highScore)
            {
                PlayerPrefs.SetInt("highscore", m_highScore);
            }

            Time.timeScale = 0;
            m_gameOverPanel.SetActive(true);
        }
        
    }

}
