using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController00 : MonoBehaviour
{
    Rigidbody2D rb;
    AudioSource sfx;
   
    [SerializeField] float force;
    [SerializeField] float delay;
    [SerializeField] float hitOffset;
    [SerializeField] GameManager manager;
    [SerializeField] AudioClip sfxPaddle;
    [SerializeField] AudioClip sfxWall;
    [SerializeField] AudioClip sfxBrick;
    

    List<string> bricks = new List<string>
    {
        { "BrickY" },
        { "BrickG" },
        { "BrickO" },
        { "BrickR" }
    };

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        sfx = GetComponent<AudioSource>();
        Invoke("LanzarPelota", delay);
        

    }
   

    private void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;
        if (bricks.Contains(tag))
        {
            sfx.clip = sfxBrick;
            sfx.Play();
            
        }
        
        else if (tag == "paddle")
        {
            sfx.clip = sfxPaddle;
            sfx.Play();
          
        }
        else if (tag == "Wall-Lateral" )
        {
            sfx.clip = sfxWall;
            sfx.Play();

           

        }

    }
   

    public void LanzarPelota()
    {

        // reseteamos la posición de salida
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero; // antes de resertear la velocidad, hay que declarar un atributo de la clase RigitBody2D 

        float dirX, dirY = -1; // posición hacia abajo en el eje Y
        dirX = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        Vector2 dir = new Vector2(dirX, dirY);
        dir.Normalize();

        // añadimos fuerza a la salida 
        rb.AddForce(dir * force, ForceMode2D.Impulse);


    }

}

