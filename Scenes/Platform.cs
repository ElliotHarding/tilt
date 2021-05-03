using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject leftMover;
    public GameObject rightMover;
    public float moverSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {       
    }

    // Update is called once per frame
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
                    float yDiff = touchPos.y - rightMover.transform.position.y;
                    rightMover.transform.position += new Vector3(0, yDiff * Time.deltaTime * moverSpeed, 0);
                }
                else
                {
                    float yDiff = touchPos.y - leftMover.transform.position.y;
                    leftMover.transform.position += new Vector3(0, yDiff * Time.deltaTime * moverSpeed, 0);
                }
            }            
        }

        //Handle mouse clicks
        if(Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

            if (mousePos.x > 0)
            {
                float yDiff = mousePos.y - rightMover.transform.position.y;
                rightMover.transform.position += new Vector3(0, yDiff * Time.deltaTime * moverSpeed, 0);
            }
            else
            {
                float yDiff = mousePos.y - leftMover.transform.position.y;
                leftMover.transform.position += new Vector3(0, yDiff * Time.deltaTime * moverSpeed, 0);
            }
        }


        //Set platform position
        Vector3 midpoint = new Vector3(0, (rightMover.transform.position.y + leftMover.transform.position.y) / 2);
        transform.position = midpoint;

        //Set platform rotation
        Vector2 diference = rightMover.transform.position - leftMover.transform.position;
        float sign = (rightMover.transform.position.y < leftMover.transform.position.y) ? -1.0f : 1.0f;
        float rotation = Vector2.Angle(Vector2.right, diference) * sign;
        transform.localEulerAngles = new Vector3(0, 0, rotation);
       
    }
}
