using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScoreForUI : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    private GameObject _flyDestination;
    Rigidbody rb;
    bool flyToHud;
    float timeStamp;
    Vector2 directionHud;
    Transform score;

    private ScoreManager scoreManager;


    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        _flyDestination = GameObject.FindGameObjectWithTag("ScoreBox");
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(flyToHud)
        {
            directionHud = -(transform.position - new Vector3(-score.position.x,score.position.y,score.position.z)).normalized;
            
            transform.LookAt(directionHud);
            rb.velocity = new Vector2 (directionHud.x,directionHud.y)*_speed*(Time.time /timeStamp);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TestUI")
        {
            timeStamp = Time.time;
            score = GameObject.Find("Image").transform;
            // flyToHud = true;

            transform.LookAt(_flyDestination.transform);
            Tween t = transform.DOMove(_flyDestination.transform.position, 3f).OnComplete(() => 
            {
                scoreManager.AddScore(GetComponent<User>());
                Destroy(gameObject);
            });

            t.SetEase(Ease.Linear);

            rb.isKinematic = false;
            GetComponent<Animator>().SetBool("Fly", true);

//            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<UserBase>().enabled = false;
            gameObject.layer = 10;
        }

        if (other.CompareTag("ScoreBox"))
        {
            Destroy(gameObject);
        }
    }
}
