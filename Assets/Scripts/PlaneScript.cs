using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float tapStrenght;
    public GameOverScript dead;
    
    // Start is called before the first frame update
    void Start()
    {
        dead = GameObject.FindGameObjectWithTag("Logic").GetComponent<GameOverScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            rb.velocity = Vector2.up * tapStrenght;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        dead.gameOver();
    }
}
