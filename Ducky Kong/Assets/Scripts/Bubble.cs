using UnityEngine;

public class Bubble : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<Player>().LoseLife();
        }
    }
}
