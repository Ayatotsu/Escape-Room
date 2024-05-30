using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class Timer : MonoBehaviour
{
    public GameManager gameManager;

    public float timeRemaining = 180f;
    public bool timeIsRunning = false;
    public bool showTimer = false;

    [Header("References")]
    public TextMeshProUGUI timeText;

    [Header("Panels")]
    public GameObject objCanvas;
    public GameObject inGamePanel;
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    [Header("GameElements")]
    public GameObject mc;
    public AudioSource explosion;


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
        if (timeRemaining <= 30f) 
        {
            timeText.color = Color.yellow;
        }
        if (timeRemaining <= 10f) 
        {
            timeText.color = Color.red;
        }

        if (timeRemaining <= 0f)
        {
            
            gameManager.bombTicking.Stop();
            explosion.Play();
            timeIsRunning = false;
            GameOver();

        }
        if (timeIsRunning)
        {

            if (timeRemaining <= 180f)
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


    //use this method later on if run out of time and set the UI video of the explosion
    public void GameOver()
    {
        if (!timeIsRunning)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            //Add something that will call out the GameOver Scene or Video
            
            inGamePanel.gameObject.SetActive(false);
            gameOverPanel.gameObject.SetActive(true);
        }
    }
}
