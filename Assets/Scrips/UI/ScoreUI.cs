using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text text;

    public void SetScoreUI(int i)
    {
        text.text = i.ToString();
    }
}
