using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class Timer : MonoBehaviour
{
    public GameManager gameManager;

    public float timeRemaining = 182;
    public bool timeIsRunning = false;
    public bool showTimer = false;

    [Header("References")]
    public TextMeshProUGUI timeText;

    [Header("Panels")]
    public GameObject objCanvas;
    public GameObject inGamePanel;
    public GameObject gameOverPanel;

    [Header("GameElements")]
    public GameObject mc;


    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        objCanvas = GameObject.Find("Canvas").gameObject;
        inGamePanel = objCanvas.transform.Find("InGamePanel").gameObject;
        gameOverPanel = objCanvas.transform.Find("GameOverPanel").gameObject;
        mc = GameObject.Find("Player").gameObject;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining <= 32f) 
        {
            timeText.color = Color.yellow;
        }
        if (timeRemaining <= 12f) 
        {
            timeText.color = Color.red;
        }

        if (timeRemaining <= 1.2f)
        {
            //GameOver();
            gameManager.bombTicking.Stop();
            timeIsRunning = false;
            //Time.timeScale = 0f;

        }
        if (timeIsRunning)
        {

            if (timeRemaining <= 182)
            {
                TimeDisplay();
                
            }

        }

    }


    public void TimeDisplay()
    {

        float timeToDisplay;
        timeToDisplay = timeRemaining -= Time.deltaTime;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if (!showTimer)
        {
            timeText.text = null;
        }
        else 
        {
            timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        }
        
        
    }


    // use this method later on if run out of time and set the UI video of the explosion
    //public void GameOver()
    //{
        //if (!timeIsRunning)
        //{
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            
            //Add something that will call out the GameOver Scene or Video
        //}
    //}
}
