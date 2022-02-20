using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int score;
    [SerializeField] ScoreUI ui;
    static Score instance;
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoadManager.DontDestroyOnLoad(this.gameObject);
            instance = this;
            ui.SetScoreUI(0);
        }
        else
        {
            Destroy(this.gameObject);
            ui.SetScoreUI(score);
        }
    }
    public int GetScore() { return score; }
    public void AddScore(int i) { score += i; ui.SetScoreUI(score); }
    public void SubtractScore(int i) { if (score > 0) score -= i; ui.SetScoreUI(score); }
    public void SetScore(int i) { score = i; ui.SetScoreUI(score); }
}
