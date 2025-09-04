using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float speed = 30f;
    private PlayerController playerController;
    private float leftBound = -15f;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    void Update()
    {
        if (playerController != null && !playerController.gameOver)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }

    }
}

