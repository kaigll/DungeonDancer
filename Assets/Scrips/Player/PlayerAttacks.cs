using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private PlayerInputs input;

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("collision");
            if (GetComponent<Enemy_Main>() != null) GetComponent<Enemy_Main>().Damage(1);
        }
    }
}
