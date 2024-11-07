using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int score { get; private set; } = 0;
    public static int lifes { get; private set; } = 3;

    public static List<int> totalBricks = new List<int>
    {0,36,29};

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public static void UpdateScore(int points) 
    { 
        score += points; 
    }
    public static void UpdateLife(int numLifes)
    {
        lifes += numLifes;

    }
    public static void SetLife(int numlife)
    {
       lifes = numlife;
    }

    public static void SetScore(int points)
    {
        score = points;
    }

    public static int GetLife()
    {
        return lifes;
    }

    
}
