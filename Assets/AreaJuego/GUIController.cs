using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    [SerializeField] Text txtScore;
    [SerializeField] Text txtLife;
    

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    void OnGUI()
    {
        txtScore.text = string.Format("{0,3:D3}", GameManager.score);
        txtLife.text = GameManager.lifes.ToString();

    }
}
