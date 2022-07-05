using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    int a = 1;
    float counter;

    [SerializeField] float ySpeed, zSpeed = 0;

    public bool canMove = true;

    Animator animator;
    Rigidbody rb;
    BoxCollider boxCollider;

    [SerializeField] GameObject particle;
    [SerializeField] ParticleSystem fallParticle;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.deltaTime);
        //Debug.Log(transform.forward);
        if (!gameManager.gameOver)
        {

            if (canMove)
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
                {
                    rb.velocity = Vector3.zero;
                    particle.SetActive(true);
                    counter += Time.deltaTime * 33;
                    if (counter > a && transform.localScale.x < 10)
                    {
                        a++;
                        transform.localScale += new Vector3(0.12f, -0.12f, 0.12f);
                        gameManager.currentPlatform.transform.localScale -= new Vector3(0, 0.24f, 0);
                    }
                    transform.position = new Vector3(transform.position.x, transform.localScale.y / 2 + gameManager.currentPlatform.transform.localScale.y, transform.position.z);
                    zSpeed += 7500 * Time.deltaTime;
                }
                if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
                {
                    canMove = false;
                    rb.useGravity = true;
                    boxCollider.enabled = false;
                    particle.SetActive(false);
                    rbMove();
                    transform.localScale = new Vector3(7.5f, 7.5f, 7.5f);
                    zSpeed = 0;
                    counter = 0;
                    a = 0;

                }
            }

        }
        else
        {
            StartCoroutine("GameEnded");
        }

    }

    private void FixedUpdate()
    {
        if (!gameManager.gameOver)
        {
            if (transform.position.y <= 104f)
            {
                if (!(transform.position.z > gameManager.nextPlatform.transform.position.z - 15 && transform.position.z < gameManager.nextPlatform.transform.position.z + 15)
                    && !(transform.position.z > gameManager.currentPlatform.transform.position.z - gameManager.currentPlatform.transform.localScale.z * 0.5 &&
                transform.position.z < gameManager.currentPlatform.transform.position.z + gameManager.currentPlatform.transform.localScale.z * 0.5) && rb.velocity.y <= 0)
                {
                    gameManager.gameOver = true;
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                }
            }
            if (transform.position.y > 120f)
            {
                boxCollider.enabled = true;
                animator.SetBool("Rotate", true);
                gameManager.currentPlatform.transform.localScale = Vector3.Lerp(gameManager.currentPlatform.transform.localScale,
                    new Vector3(gameManager.currentPlatform.transform.localScale.x, 100, gameManager.currentPlatform.transform.localScale.z), 0.1f);
            }
            else
            {
                animator.SetBool("Rotate", false);
            }
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        rb.useGravity = false;
        if (!gameManager.gameOver)
        {
            fallParticle.Play();
            fallParticle.Stop();
        }
        if (other.gameObject != gameManager.currentPlatform)
        {
            rb.velocity = Vector3.zero;
        }
        if (transform.position.z > gameManager.nextPlatform.transform.position.z - 15 && transform.position.z < gameManager.nextPlatform.transform.position.z + 15)
        {
            if (transform.position.z > gameManager.nextPlatform.transform.position.z - 2 && transform.position.z < gameManager.nextPlatform.transform.position.z + 2)
            {
                gameManager.Scored(2);
            }
            else
            {
                gameManager.Scored(1);
            }
            gameManager.currentPlatform = other.gameObject;
            gameManager.NewPlatform();

        }
        rb.velocity = Vector3.zero;
        canMove = true;
    }

    private void OnCollisionExit(Collision collision)
    {

    }

    void rbMove()
    {
        rb.AddForce(0, ySpeed, -zSpeed);
    }

    public IEnumerator GameEnded()
    {
        yield return new WaitForSeconds(0.5f);
        rb.useGravity = true;
        yield return new WaitForSeconds(1f);
        gameManager.Ended();
    }

}
