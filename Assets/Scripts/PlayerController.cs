using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float speedModifier = 1.5f;
    [SerializeField] private float gravityModifier = 1f;

    [Header("FX")]
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private ParticleSystem dirtParticles;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip crashSound;

    private Rigidbody playerRb;
    private Animator playerAnimator;
    private AudioSource playerAudio;

    private bool isOnGround = true;
    public bool gameOver { get; private set; }
    private bool queuedJump;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAnimator.speed *= speedModifier;
        if (explosionParticles == null || dirtParticles == null)
        {
            Debug.LogAssertion("Missing Particle Sysytem!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            queuedJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (!queuedJump) return;
        queuedJump = false; // reset flag

        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
        playerAnimator.SetTrigger("Jump_trig");
        if (dirtParticles) dirtParticles.Stop();
        if (jumpSound) playerAudio.PlayOneShot(jumpSound, 1.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground") && !gameOver)
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
            if (explosionParticles) explosionParticles.Play();
            if (dirtParticles) dirtParticles.Stop();
            if (crashSound) playerAudio.PlayOneShot(crashSound, 1.0f);

        }

    }
}
