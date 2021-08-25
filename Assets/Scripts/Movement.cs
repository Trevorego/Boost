using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;

    [SerializeField] float MainThrust = 1000f;
    [SerializeField] float RotationThrust = 1f;
    [SerializeField] float RotationRate = 1f;
    [SerializeField] AudioClip MainEngineThrust;
    [SerializeField] ParticleSystem MainEngineThrustParticle;
    [SerializeField] ParticleSystem SideEngineThrustParticle1;
    [SerializeField] ParticleSystem SideEngineThrustParticle2;

    bool LeftEngine = false;
    bool RightEngine = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
        
        Vector3 andac3 = transform.rotation.eulerAngles;
        Debug.Log(andac3.z);
        if (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y) + Mathf.Abs(rb.velocity.z) < 3f)
            return;
        if (andac3.z >= 180)
            andac3.z += (Mathf.Atan2(rb.velocity.x, rb.velocity.y) * Mathf.Rad2Deg - andac3.z) * Time.deltaTime / RotationRate;
        else
            andac3.z -= (Mathf.Atan2(rb.velocity.x, rb.velocity.y) * Mathf.Rad2Deg - andac3.z) * Time.deltaTime / RotationRate;
        transform.rotation = Quaternion.Euler(andac3);
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * Time.deltaTime * MainThrust);
            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(MainEngineThrust);
                MainEngineThrustParticle.Play();
            }
        }
        else
        {
            audioSource.Stop();
            MainEngineThrustParticle.Stop();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(RotationThrust);
            if (!RightEngine)
            {
                SideEngineThrustParticle1.Play();
                RightEngine = true;
            }
        }
        else
        {
            SideEngineThrustParticle1.Stop();
            RightEngine = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-RotationThrust);
            if (!LeftEngine)
            {
                SideEngineThrustParticle2.Play();
                LeftEngine = true;
            }
        }
        else
        {
            SideEngineThrustParticle2.Stop();
            LeftEngine = false;
        }
    }

    void ApplyRotation(float RotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * RotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}