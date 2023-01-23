using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public  class PlayerController: MonoBehaviour
{
    bool stopped = false;
    public bool chupa = false;
    public double bites = 0;
    int[] values = new int[4];
    public int diceAmount = 4;
    private DiceScript[] dice;
    public GameObject DiceDemo1;
    public GameObject DiceDemo2;
    public GameObject DiceDemo3;
    public GameObject DiceDemo4;

    public TextMeshProUGUI bitesText;
    AudioSource audioSource;
    public AudioClip biteClip;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        bitesText.enabled = false;
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
    void Update()
    {

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
        bool chickens = false;
        int numChick = 0;
        int i, j, min;
        DiceScript temp;
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
            for(i = 0; i < 4; i++){
                if(values[i] == 1 || values[i] == 2){
                    numChick++;
                }
            }
            if(numChick >= 2){
                chickens = true;
            }
            i = 0;
            while(consume > 0){
                if(dice[i].value == -1){
                    i++;
                    continue;
                }
                if(chickens == true){
                    PlaySound(biteClip);
                    i++;
                    dice[i-1].eaten = true;
                    dice[i-1].anim.SetInteger("Roll Value", 7);
                    dice[i-1].Clicked();
                    dice[i-1].value = -1;
                    consume = consume - .5;
                    PlaySound(biteClip);
                    dice[i].eaten = true;
                    dice[i].anim.SetInteger("Roll Value", 7);
                    dice[i].Clicked();
                    dice[i].value = -1;
                    consume = consume - .5;
                    chickens = false;
                }
                else{
                    if(dice[i].value < 5){
                        PlaySound(biteClip);
                        dice[i].eaten = true;
                        dice[i].anim.SetInteger("Roll Value", 7);
                        dice[i].Clicked();
                        dice[i].value = -1;
                        consume--;
                    }
                    else if(dice[i].value == 5 && consume >= 2){
                        PlaySound(biteClip);
                        dice[i].eaten = true;
                        dice[i].anim.SetInteger("Roll Value", 7);
                        dice[i].Clicked();
                        dice[i].value = -1;
                        consume = consume -2;
                    }
                }
                i++;
                if(i >=4)
                    consume = 0;
            }
        }
        return 0;
    }
}
