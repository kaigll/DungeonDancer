using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    internal float horizontal;
    internal float vertical;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) vertical = 1f;
        if (Input.GetKeyDown(KeyCode.S)) vertical = -1f;
        if (Input.GetKeyDown(KeyCode.D)) horizontal = 1f;
        if (Input.GetKeyDown(KeyCode.A)) horizontal = -1f;
        Debug.Log(horizontal + ", " + vertical);
    }
}
