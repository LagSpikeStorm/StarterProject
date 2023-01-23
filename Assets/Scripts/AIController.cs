using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public  class AIController : MonoBehaviour, IPointerClickHandler
{
    bool stopped = false;
    public bool chupa = false;
    public double bites = 0;
    public int diceAmount = 4;
    int[] values = new int[4];
    public bool playerTurn = false;
    double playerBites = 0;
    int bigly = 0;
    bool bigTest = false;

    private DiceScript[] dice;

    public GameObject DiceDemo1;
    public GameObject DiceDemo2;
    public GameObject DiceDemo3;
    public GameObject DiceDemo4;

    private Camera mainCamera;

    public TextMeshProUGUI bitesText;
    AudioSource audioSource;
    public AudioClip biteClip;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        bitesText.enabled = false;
        mainCamera = Camera.main;
        dice = new DiceScript[4];
        dice[0] = DiceDemo1.GetComponent<DiceScript>();
        dice[1] = DiceDemo2.GetComponent<DiceScript>();
        dice[2] = DiceDemo3.GetComponent<DiceScript>();
        dice[3] = DiceDemo4.GetComponent<DiceScript>();
        Invoke(nameof(activateText), 2);
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    void activateText(){
        bitesText.enabled = true;
    }
    public void deactivateText(){
        bitesText.enabled = false;
    }

    public void textUp(){
        bitesText.text = "Bites:" + bites.ToString();
    }

    // Update is called once per frame
    public void Roll(){
        dice[0] = DiceDemo1.GetComponent<DiceScript>();
        dice[1] = DiceDemo2.GetComponent<DiceScript>();
        dice[2] = DiceDemo3.GetComponent<DiceScript>();
        dice[3] = DiceDemo4.GetComponent<DiceScript>();
        if(stopped == false){
            for(int i = 0; i < 4; i++){
                dice[i].Stop();
                values[i] = dice[i].value;
            }
            for(int i = 0; i < 4; i++){
                if(values[i] == 6){
                    bites++;
                    chupa = true;
                }
            }
            textUp();
            stopped = true;
        }
        else if(stopped == true){
            for(int i = 0; i < 4; i ++){
                dice[i].anim.SetInteger("Roll Value", 0);
            }
            stopped = false;
            bites = 0;
            textUp();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
    }
    void Update()
    {
        bool play = true;
        GameObject tempDice;
        DiceScript temp;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        int j;
        if(playerTurn){
            if(playerBites < 3){
                for(j = 0; j < 4; j++){
                    if(dice[j].value > 5 || dice[j].value < 0){
                        play = false;
                    }
                    else if(dice[j].value < 6 && dice[j].value > 0){
                        play = true;
                        break;
                    }
                }
                if(!play){
                    playerBites = 0;
                    playerTurn = false;
                }

            }
            if(playerBites < 2){
                for(j = 0; j < 4; j++){
                    if(dice[j].value > 4 || dice[j].value < 0){
                        play = false;
                    }
                    else if(dice[j].value < 5 && dice[j].value > 0){
                        play = true;
                        break;
                    }
                }
                if(!play){
                    playerBites = 0;
                    playerTurn = false;
                }
            }
            if(playerBites > 0 && playerBites < 1){
                Debug.Log("works");
                for(j = 0; j < 4; j++){
                    if(dice[j].value == 1 || dice[j].value == 2){
                        play = true;
                        break;
                    }
                    else{
                        play = false;
                    }
                }
                if(!play){
                    playerBites = 0;
                    playerTurn = false;
                }
            }
            if(playerBites == 0){
                playerBites = 0;
                playerTurn = false;
            }
            if(hit.collider != null){
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if(Input.GetMouseButtonDown(0)){
                    tempDice = hit.collider.gameObject;
                    temp = tempDice.GetComponent<DiceScript>();
                    Debug.Log(temp.value);
                    if(temp.value == 1){
                        PlaySound(biteClip);
                        playerBites = playerBites - .5;
                        temp.eaten = true;
                        temp.anim.SetInteger("Roll Value", 7);
                        temp.Clicked();
                        temp.value = -1;
                    }
                    else if(temp.value == 2){
                        PlaySound(biteClip);
                        playerBites = playerBites - .5;
                        temp.eaten = true;
                        temp.anim.SetInteger("Roll Value", 7);
                        temp.Clicked();
                        temp.value = -1;
                    }
                    else if(temp.value > 2 && temp.value < 5 && playerBites > .5){
                        PlaySound(biteClip);
                        playerBites--;
                        temp.eaten = true;
                        temp.anim.SetInteger("Roll Value", 7);
                        temp.Clicked();
                        temp.value = -1;
                    }
                    else if(temp.value == 5 && playerBites >= 2){
                        PlaySound(biteClip);
                        playerBites = playerBites - 2;
                        temp.eaten = true;
                        temp.anim.SetInteger("Roll Value", 7);
                        temp.Clicked();
                        temp.value = -1;
                    }
                }
            }
            
        }
    }
    public int Count(){
        diceAmount = 4;
        for(int i = 0; i < 4; i++){
            if(dice[i].value == -1){
                diceAmount--;
            }
        }
        return diceAmount;
    }
    public int Bites(bool eat, double consume){ 

        DiceScript temp;
        int i, j, min;
        int bigly = 0;
        if(eat){
            for(i = 0;i < 4; i++){
                min = i;
                for(j = i+1; j < 4;j++){
                    if(dice[j].value < dice[min].value)
                        min = j;
                }
                if(min != i){
                    temp = dice[min];
                    dice[min] = dice[i];
                    dice[i] = temp;
                }
            }
            playerTurn = true;
            playerBites = consume;
            bigTest = true;
        }
        return 0;
    }
}
