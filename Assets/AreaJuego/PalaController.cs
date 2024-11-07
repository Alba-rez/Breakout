using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PalaController : MonoBehaviour
{
    [SerializeField] float speed;
    const float MIN_X = -3.15f;
    const float MAX_X = 3.11f;
    void Start()
    {
        
    }

    
    void Update()
    {

        
        if (Input.GetKey("right") && transform.position.x<MAX_X)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if(Input.GetKey("left") && transform.position.x > MIN_X)
{
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

    }
}
