using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool ignoreNextCollision;
    private Rigidbody ballRb;
    public float impulseForce = 5f;
    private Vector3 startPos;
    public int perfectPass = 0;
    public bool isSuperSpeedActive = false;
    public ParticleSystem superParticle;

    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        startPos = transform.position;
        superParticle.Stop();
    }



    private void OnCollisionEnter(Collision other)
    {
        if (ignoreNextCollision)
        {
            return;
        }

        if (isSuperSpeedActive)
        {
            if (!other.transform.GetComponent<Goal>())
            {
                Destroy(other.transform.parent.gameObject);
            }
        }
        else
        {
            DeathPart deathPart = other.transform.GetComponent<DeathPart>();

            if (deathPart)
            {
                deathPart.HitDeathPart();
                return;
            }
        }


        ballRb.velocity = Vector3.zero;
        ballRb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

        ignoreNextCollision = true;
        Invoke(nameof(AllowCollision), .2f);
        perfectPass = 0;
        isSuperSpeedActive = false;
        superParticle.Stop();
    }

    private void AllowCollision()
    {
        ignoreNextCollision = false;
    }

    public void ResetBall()
    {
        transform.position = startPos;
    }

    // Update is called once per frame
    private void Update()
    {
        if (perfectPass >= 3 && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;
            superParticle.Play();
            // transform.GetComponent<ParticleSystem>().gameObject.SetActive(true);
            ballRb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }
    }
}
