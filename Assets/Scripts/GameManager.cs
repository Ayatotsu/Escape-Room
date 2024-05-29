using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Rendering;
using System.Linq;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    //Item List
    public Item[] items;
    //Camera reference
    public CameraLook fps;
    //Canvas Text
    public TMP_Text objectText;
    public TMP_Text dialogueText;
    public TMP_Text interactText;
    //Light
    public Light lightsOn;

    //Timer
    public AudioSource bombTicking;
    public Timer timer;
    public GameObject timerObject;
    
    //Mission
    public GameObject mission;
    public TMP_Text missionText;

    public LayerMask layerMask;

    
    public bool hasKeys = false;
    public bool scissors;
    public bool flashLight;
    public bool battery;
    public bool bomb = false;
    public bool disarmed = false;
    public GameObject doorObj;
    public GameObject door2;
    public Door door;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Dialogue());
    }

    // Update is called once per frame
    void Update()
    {
        HitObject();
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2f, Color.green);
        
    }

    IEnumerator Dialogue() 
    {
        yield return new WaitForSeconds(3f);

        dialogueText.text = "What happened? I don't feel good...";


        yield return new WaitForSeconds(3f);
        bombTicking.Play();
        dialogueText.text = "What is that sound, Make it stop!";
        timerObject.SetActive(true);
        timer.timeIsRunning = true;
        mission.SetActive(true);
        missionText.text = "Find the source of the sound and stop it.";
        StartCoroutine(DisableDialogue());

    }
    IEnumerator DisableDialogue() 
    {
        yield return new WaitForSeconds(2f);
        dialogueText.text = null;
    }
    public void HitObject()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        
        //Raycast Hit Objects (Items and Door Layers)
        if (Physics.Raycast(ray, out hit, 2f, layerMask))
        {
            objectText.text = hit.collider.name.ToString();
            interactText.text = "Press 'E' to interact";
            Debug.Log(hit.collider.name);
        }
        else
        {
            objectText.text = null;
            interactText.text = null;
        }

        //Key Interaction
        if (Input.GetKeyDown(KeyCode.E) && hit.collider.name == "Keys" && flashLight == false && battery == false)
        {
            dialogueText.text = "I still need to find the flashlight and the battery first";
            StartCoroutine(DisableDialogue());
        }
        if (Input.GetKeyDown(KeyCode.E) && hit.collider.name == "Keys" && flashLight == true && battery == false)
        {
            dialogueText.text = "I still need to find the battery first";
            StartCoroutine(DisableDialogue());
        }
        else if (Input.GetKeyDown(KeyCode.E) && hit.collider.name == "Keys" && flashLight == true && battery == true) 
        {
            Inventory.instance.AddItem(items[0]);
            hasKeys = true;
            Destroy(hit.collider.gameObject);
        }

        //Scissors Interaction
        if (Input.GetKeyDown(KeyCode.E) && hit.collider.name == "Scissors" && bomb == false)
        {
            dialogueText.text = "I can't use this for now.";
            StartCoroutine(DisableDialogue());
        }
        else if (Input.GetKeyDown(KeyCode.E) && hit.collider.name == "Scissors" && bomb == true)
        {
            Inventory.instance.AddItem(items[1]);
            scissors = true;
            dialogueText.text = "I think I can use this to cut wires.";
            StartCoroutine(DisableDialogue()); 
            missionText.text = "Disarm the bomb.";
            Destroy(hit.collider.gameObject);
        }

        //Flashlight Interaction
        if (Input.GetKeyDown(KeyCode.E) && hit.collider.name == "Flashlight")
        {
            Inventory.instance.AddItem(items[2]);
            flashLight = true;
            Destroy(hit.collider.gameObject);
        }

        //Battery Interaction
        if (Input.GetKeyDown(KeyCode.E) && hit.collider.name == "Battery" && flashLight == false)
        {
            dialogueText.text = "I still need to find the flashlight first";
            StartCoroutine(DisableDialogue());

        }
        else if (Input.GetKeyDown(KeyCode.E) && hit.collider.name == "Battery" && flashLight == true)
        {
            Inventory.instance.AddItem(items[3]);
            battery = true;
            Destroy(hit.collider.gameObject);

        }

        //Open light
        if (Input.GetKeyDown(KeyCode.F) && battery == false && flashLight == true)
        {
            dialogueText.text = "I need to find the battery first.";
            StartCoroutine(DisableDialogue());
        }
        else if (Input.GetKeyDown(KeyCode.F) && battery == true && flashLight == true)
        {
            lightsOn.gameObject.SetActive(true);
        }

        //Door Interaction
        if (Input.GetKeyDown(KeyCode.E) && !hasKeys && hit.collider.name == "Door2")
        {
            dialogueText.text = "I can't go out there yet, I still need to do something.";
            StartCoroutine(DisableDialogue());
        }
        else if (Input.GetKeyDown(KeyCode.E) && hasKeys && hit.collider.name == "Door2")
        {
            if (door.IsOpen == false)
            {
                door.Open(transform.position);
            }
            else 
            {
                door.Close();
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && hit.collider.name == "Door1" && hasKeys && disarmed == false)
        {
            dialogueText.text = "I can't go out there yet, I still need to do something.";
            StartCoroutine(DisableDialogue());
        }
        else if (Input.GetKeyDown(KeyCode.E) && hit.collider.name == "Door1" && hasKeys && disarmed == true)
        {
            //Add some Game Complete Scene or UI Video.
            //then disable some of the things you want.
        }

        //Bomb Got Hit by Raycast
        if (Input.GetKeyDown(KeyCode.E) && hit.collider.name == "Bomb" && scissors == false)
        {
            dialogueText.text = "A bomb!? I need to find a way to disarm it.";
            StartCoroutine(DisableDialogue());
            bomb = true;
            //timer.TimeDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.E) && hit.collider.name == "Bomb" && scissors == true) 
        {
            timer.timeIsRunning = false;
            timerObject.SetActive(false);
            disarmed = true;
            bombTicking.Stop();
            missionText.text = "Get out of the house.";
            dialogueText.text = "Now I have to get out of here";
            StartCoroutine(DisableDialogue());
        }




    }
    

}


[System.Serializable]

public class Item 
{
    
    public string name;
    public int count;

    public Item(string itemName, int itemCount)
    {
        name = itemName;
        count = itemCount;
    }
}
