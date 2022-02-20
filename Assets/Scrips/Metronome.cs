using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Metronome : MonoBehaviour
{
    public float songBpm;
    public float secPerBeat;
    public float songPosition;
    public float songPositionInBeats;
    public float dspSongTime;
    public AudioSource musicSource;
    public float firstBeatOffset;

    public float beatsPerLoop;
    public int completedLoops = 0;
    public float loopPositionInBeats;

    public float loopPositionInAnalog;
    public static Metronome instance;
    public float beat;
    public bool beatEvent;
    private bool playerActionTaken;
    private int failCount;


    MetronomeUI ui;
    GameObject player;
    PlayerMovement playerMovement;
    PlayerHealth playerHealth;
    Score score;

    [SerializeField] private int timingForgiveness;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoadManager.DontDestroyOnLoad(this.gameObject);
            instance = this;
        } else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        GetComponents();
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }
    void GetComponents()
    {
        ui = FindObjectOfType<Canvas>().GetComponentInChildren<MetronomeUI>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerHealth = player.GetComponentInParent<PlayerHealth>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        musicSource = GetComponent<AudioSource>();
    }
    bool AnyObjectNull()
    {
        bool var = false;
        if (ui == null) var = true;
        if (player == null) var = true;
        if (playerMovement == null) var = true;
        if (playerHealth == null) var = true;
        if (musicSource == null) var = true;
        return var;
    }

    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = songPosition / secPerBeat;
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop) completedLoops++;
        loopPositionInBeats = (songPositionInBeats - completedLoops * beatsPerLoop) + 1;
        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
        beat = loopPositionInBeats - (int)loopPositionInBeats;

        if (ui != null)
        {
            ui.SetMetronomeBar(beat);
        }
        if ((Mathf.Round(beat * 10)) / 10 == 1) beatEvent = true;
        else { beatEvent = false; playerActionTaken = false; }

        if (player != null && playerMovement != null && beatEvent && !playerActionTaken)
        {
            if (playerMovement.move || playerMovement.movingCount < timingForgiveness)
            {
                score.AddScore(1);
                playerActionTaken = true;
            } else
            {
                failCount++;
                if (failCount > 2) { score.SubtractScore(1); failCount = 0; }
                playerActionTaken = true;
            }
        }
        if (AnyObjectNull()) GetComponents();
    }
}
