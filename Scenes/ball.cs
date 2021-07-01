using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    public GameObject m_gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name.Contains("wall") || col.gameObject.name.Contains("Spike") || col.gameObject.name.Contains("Beam"))
        {
            Time.timeScale = 0;
            m_gameOverPanel.SetActive(true);
        }
        
    }

}
