using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetronomeUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public float currentValue;
    static MetronomeUI instance;
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoadManager.DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetMetronomeBar(float f)
    {
        slider.value = f;
    }
}
