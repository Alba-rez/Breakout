using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    Rigidbody2D rb;
    AudioSource sfx;
    int count = 0;
    GameObject paddle;
    bool halved;
    int brickCount;
    int sceneId;

    [SerializeField] float force;
    [SerializeField] float delay;
    [SerializeField] float hitOffset;
    [SerializeField] GameManager manager;
    [SerializeField] AudioClip sfxPaddle;
    [SerializeField] AudioClip sfxWall;
    [SerializeField] AudioClip sfxBrick;
    [SerializeField] AudioClip sfxFail;
    [SerializeField] AudioClip sfxNextLevel;
    [SerializeField] float forceInc;
    [SerializeField] Text txtgameOver;

    Dictionary<string, int> bricks = new Dictionary<string, int>
    {
        { "BrickY",10 },
        { "BrickG",15 },
        { "BrickO",20 },
        { "BrickR",25 },
        {"Brick-Pass",25 }
    };

    void Start()
    {
        sceneId = SceneManager.GetActiveScene().buildIndex;
        rb = GetComponent<Rigidbody2D>();
        sfx = GetComponent<AudioSource>();
        Invoke("LanzarPelota", delay);
        paddle = GameObject.FindWithTag("paddle");
        txtgameOver.gameObject.SetActive(false);

    }
    void Update()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall-Bottom")
        {
            sfx.clip = sfxFail;
            sfx.Play();
            GameManager.UpdateLife(-1);
            GameManager.GetLife();

            //manager.GetLife();

            if (GameManager.GetLife() > 0)
            {
                if (halved)
                {
                    HalvePaddle(false);
                }

                Invoke("LanzarPelota", delay);

            }
            else if (GameManager.GetLife() == 0)
            {
                CancelInvoke("LanzarPelota");
                txtgameOver.gameObject.SetActive(true);
                //GameManager.SetLife(3); -> esto era para poder probar el juego 
                //Invoke("LanzarPelota", 3);
                Invoke("SameScene",3);
                
                
            }
        }
        else if (other.tag == "Brick-Pass")
        {
            DestroyBrick(other.gameObject);

            if (GameManager.GetLife() < 3)
            {
                GameManager.SetLife(1);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
       
        string tag = other.gameObject.tag;
        if (bricks.ContainsKey(tag))
        {
            DestroyBrick(other.gameObject);
        }
    
        else if (tag == "BrickRE")
        {
           
            if (GameManager.GetLife() < 3)
            {
                sfx.clip = sfxBrick;
                sfx.Play();
                GameManager.UpdateLife(+1);
                Destroy(other.gameObject);
            }
           
        }
        else if (tag == "paddle")
        {
            sfx.clip = sfxPaddle;
            sfx.Play();
            // paddle position
            Vector3 paddle = other.gameObject.transform.position;

            //get the contact point

            Vector2 contact = other.GetContact(0).point;

            if ((rb.velocity.x < 0 && contact.x > (paddle.x + hitOffset)) || (rb.velocity.x > 0 && contact.x < (paddle.x) + hitOffset))
            {
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            }
            count++;

            if (count % 4 == 0)
            {
                rb.AddForce(rb.velocity.normalized * forceInc, ForceMode2D.Impulse);
            }

        }
        else if (tag == "Wall-Lateral" || tag == "Wall-Top" || tag=="Brick-Rock")
        {
            sfx.clip = sfxWall;
            sfx.Play();

            if (!halved && tag == "Wall-Top")
            {
                HalvePaddle(true);
            }

        }

    }
    void HalvePaddle(bool halve)
    {
        halved = halve;
        Vector3 scale = paddle.transform.localScale;
        paddle.transform.localScale = halved ? new Vector3(scale.x * 0.5f, scale.y, scale.z) : new Vector3(scale.x * 2f, scale.y, scale.z);
        // si halved es true, es decir, toca wall-top, mulriplica la escala de la pala x 0.5 y así la reduce a la mitad, en cambio si halved es false
        // multiplica la escala de la pala x 2( siempre que ya esté reducida), ya que ha perdido 1 vida, y así volvería a su tamaño original
    }

    public void LanzarPelota()
    {

        // reseteamos la posición de salida
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero; // antes de resertear la velocidad, hay que declarar un atributo de la clase RigitBody2D 

        float dirX, dirY = -1; // posición hacia abajo en el eje Y
        dirX = UnityEngine.Random.Range(0, 2) == 0 ? -1f : 1f;
        Vector2 dir = new Vector2(dirX, dirY);
        dir.Normalize();

        // añadimos fuerza a la salida 
        rb.AddForce(dir * force, ForceMode2D.Impulse);


    }
    void DestroyBrick(GameObject obj)
    {
        sfx.clip = sfxBrick;
        sfx.Play();
        GameManager.UpdateScore(bricks[obj.tag]);
        Destroy(obj);
        
        ++brickCount;

        if (brickCount == GameManager.totalBricks[sceneId])
        {
            rb.velocity = Vector2.zero; // para que la pelota no se siga moviendo antes de que salte a la siguiente escena
            GetComponent<SpriteRenderer>().enabled = false;
            Invoke("NextScene", 3);
            
        }

    }
    void NextScene()
    {
        int nextId = sceneId + 1;
        if (nextId == GameManager.totalBricks.Count)
        {
            nextId = 0;
        }
        SceneManager.LoadScene(nextId);
        if (GameManager.GetLife() < 3)
        {
            GameManager.SetLife(3);
        }


    }

    void SameScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        GameManager.SetLife(3);
        GameManager.SetScore(0);
    }


}

