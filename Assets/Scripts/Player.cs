using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    bool SwipeLeft;
    bool SwipeRight;
    [HideInInspector]
    bool SwipeUp;
    public float speed = 0.2f;
    private int[] lines = {-8, -4, 0};
    private int currentLane = 1;
    private float x = 0;
    public float JumpPower = 8f;
    private float y;
    private CharacterController m_char;
    private Animator m_animator;
    public bool InJump;

    private void Start() {
        transform.position = new Vector3(0 , 0 , lines[currentLane]);
        m_char = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Car")
        {
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
            SceneManager.LoadScene("Menu");
        }
    }

    private void Update() {
        transform.localEulerAngles = new Vector3(0, -90, 0);
        SwipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        SwipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        SwipeUp = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
        if (SwipeLeft)
        {   m_animator.Play("TurnLeft");
            if ( currentLane > 0 ){
                currentLane -= 1;
            }
        }
        else if (SwipeRight)
        {   m_animator.Play("TurnRight");
            if ( currentLane < 2 ){
                currentLane += 1;
            }
        }
        x -= speed;
        transform.position = new Vector3(x , y*Time.deltaTime , lines[currentLane]);   
    }


    public void jump()
    {
        if (m_char.isGrounded)
        {
            if(m_animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
            {
                m_animator.Play("Fall");
                InJump = false;
            }
            if (SwipeUp)
            {
                y = JumpPower;
                m_animator.CrossFadeInFixedTime("BasicMotions@jump" , 0.1f);
                InJump = true;
            } else
            {
                y -= JumpPower * 2 * Time.deltaTime;
                m_animator.Play("Fall");
            }   
        }
    }
}