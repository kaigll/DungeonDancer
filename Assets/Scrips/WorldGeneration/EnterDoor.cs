using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnterDoor : MonoBehaviour
{
    [SerializeField] string direction;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                GameObject dungeon = GameObject.FindGameObjectWithTag("Dungeon");
                WorldGeneration worldGen = dungeon.GetComponent<WorldGeneration>();
                Room room = worldGen.CurrentRoom();
                PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
                player.SetDoorDirection(direction);
                worldGen.MoveToRoom(room.Neighbor(this.direction));
                SceneManager.LoadScene("World");
            }
            else collision.gameObject.GetComponent<PlayerMovement>().StopMove();
        }
    }
    
    // THIS IS THE FIXING PART
}
