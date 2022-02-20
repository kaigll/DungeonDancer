using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReachGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().SetDoorDirection(null);
                GameObject dungeon = GameObject.FindGameObjectWithTag("Dungeon");
                WorldGeneration gen = dungeon.GetComponent<WorldGeneration>();
                gen.ResetDungeon();
                SceneManager.LoadScene("Menu");
            }
            else collision.gameObject.GetComponent<PlayerMovement>().StopMove();
        }
    }
}
