using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;


    Rigidbody rigidBody;
    AudioSource rocketThrustSound;

    enum State {Alive, Dying, Transcending }
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rocketThrustSound = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            Thrust();            Rotate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state == State.Alive) { return; } // Ignore collisions when dead

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // Do nothing
                break;

            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); // Parameterise time
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f);  // Parameterise time
                break;
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // TODO Allow for more than two levels
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) // Can thrust while rotating
        {
            float mainThrustThisFrame = mainThrust * Time.deltaTime;

            rigidBody.AddRelativeForce(Vector3.up * mainThrustThisFrame);

            if (!rocketThrustSound.isPlaying)
            {
                rocketThrustSound.Play();
            }
        }
        else
        {
            rocketThrustSound.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // Take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // Resume physics control of rotation
    }
}
