using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerGun : MonoBehaviour
{
    private float m_time = 0.0f;
    public float lazerToggleFrequency = 1f;
    public GameObject m_beam;

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;

        if (m_time >= lazerToggleFrequency)
        {
            m_time = 0.0f;

            if(m_beam.active)
            {
                m_beam.SetActive(false);
            }
            else
            {
                m_beam.SetActive(true);
            }
        }
    }
}
