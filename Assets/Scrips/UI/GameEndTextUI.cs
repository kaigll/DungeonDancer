using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndTextUI : MonoBehaviour
{
    [SerializeField] private Text textBox;
    public void Defeat()
    {
        textBox.text = "Game Over";
    }

    public void Victory()
    {
        textBox.text = "Victory";
    }
}
