using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnimator;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float speedModifier = 1.5f;
    [SerializeField] private float gravityModifier;
    [SerializeField] bool isOnGround = true;
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private ParticleSystem dirtParticles;
    public bool gameOver = false;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
        playerAnimator.speed *= speedModifier;
        if(explosionParticles == null || dirtParticles == null) { Debug.LogAssertion("Missing Particle Sysytem!"); }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnimator.SetTrigger("Jump_trig");
            dirtParticles.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticles.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            Debug.Log("Game Over!");
            playerAnimator.SetBool("Death_b", true);
            playerAnimator.SetInteger("DeathType_int", Random.Range(1, 3));
            explosionParticles.Play();
            dirtParticles.Stop();
            
        }
        
    }
}
