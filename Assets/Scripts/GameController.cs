using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    PlayerController player;
    AIController opp;
    public GameObject PC;
    public GameObject AI;
    bool stopped;
    int rounds = 6;
    bool gameEnd = false;

    int pcDice = 4;
    int aiDice = 4;

    public GameObject titleCard;
    AudioSource audioSource;

    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI roundText;

    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI drawText;
    public TextMeshProUGUI diceText;

    public AudioClip intro;
    public AudioClip bGM;
    public AudioClip loseSong;
    public AudioClip winSong;
    public AudioClip drawSong;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlaySound(intro);
        roundText.enabled = false;
        diceText.enabled = false;
        player = PC.GetComponent<PlayerController>();
        opp = AI.GetComponent<AIController>(); 
        Invoke(nameof(Destroy), 2);
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            if(player.chupa == false){
                Spin();
                rounds--;
                if(gameEnd == true){
                    audioSource.Stop();
                    player.deactivateText();
                    opp.deactivateText();
                    roundText.enabled = false;
                    diceText.enabled = true;
                    titleCard.SetActive(true);
                    pcDice = player.Count();
                    aiDice = opp.Count();
                    diceText.text = "You have: " + pcDice.ToString() + "         Opponent has: " + aiDice.ToString();
                    if(pcDice > aiDice){
                        //audioSource.clip = winSong;
                        winText.enabled = true;
                        PlaySound(winSong);
                    }
                    else if(pcDice < aiDice){
                        //audioSource.clip = loseSong;
                        loseText.enabled = true;
                        PlaySound(loseSong);
                    }
                    else if(pcDice == aiDice){
                        //audioSource.clip = drawSong;
                        drawText.enabled = true;
                        PlaySound(drawSong);
                    }
                }
            }   
        }
        if(opp.playerTurn == false){
            player.chupa = false;
            if(rounds == 0){
                gameEnd = true;
            }
            if(player.Count() == 0 || opp.Count() == 0){
                gameEnd = true;
            }
            
        }
        opp.textUp();
        player.textUp();
    }
    void Spin(){
        if(rounds%2 == 0){
            int roundDisplay = rounds/2;
            roundText.text = "Rounds:" + roundDisplay.ToString();
        }
        player.Roll();
        opp.Roll();
        opp.bites = player.Bites(opp.chupa, opp.bites);
        opp.chupa = false;
        opp.Bites(player.chupa, player.bites);  
    }
    void Destroy(){
        audioSource.Stop();
        roundText.enabled = true;
        titleCard.SetActive(false);
        Destroy(tutorialText);
        PlaySound(bGM);
    }
}
