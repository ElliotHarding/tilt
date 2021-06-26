using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
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
        if(col.gameObject.name.Contains("Platform"))
        {
            GameObject platform = col.gameObject;

            //find how close we are
            float yDistBetweenBallAndPlatform = transform.position.y - platform.transform.position.y;
            float requiredYDistance = platform.transform.lossyScale.y / 2 + transform.lossyScale.y / 2;

            float correctionDifference = requiredYDistance - yDistBetweenBallAndPlatform;

            if(correctionDifference > 0) //todo remove
            {
                Debug.Log("-----");
                Debug.Log(yDistBetweenBallAndPlatform);
                Debug.Log(requiredYDistance);

                Debug.Log(correctionDifference);

                transform.position = transform.position + new Vector3(0, correctionDifference);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        characterInQuicksand = false;
    }
}
