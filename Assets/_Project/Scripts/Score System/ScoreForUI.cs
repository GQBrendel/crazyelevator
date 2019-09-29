using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScoreForUI : MonoBehaviour
{
    [SerializeField] private GameObject _user;
    [SerializeField] private float _timeToReachScore = 2f;
    private FloorUsersSetup _finalFloor;

    private Transform _flyDestination;
    Rigidbody rb;
    bool flyToHud;
    float timeStamp;
    Vector2 directionHud;

    private Transform _rootTransform;

    private ScoreManager _scoreManager;
   

    void Start()
    {
        _rootTransform = GetComponentInParent<Transform>();
        _scoreManager = FindObjectOfType<ScoreManager>();
        _flyDestination = GameObject.FindGameObjectWithTag("ScoreBox").transform;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TestUI")
        {
            _finalFloor = other.GetComponent<FloorUsersSetup>();

            _flyDestination = _finalFloor.GetUserPosition();

            timeStamp = Time.time;

            _rootTransform.LookAt(_flyDestination.transform);
            Tween t = _rootTransform.DOMove(_flyDestination.transform.position, .5f).OnComplete(() => 
            {
                _scoreManager.AddScore(GetComponent<UserBase>());
                StartCoroutine(SelfDestroy());
                GetComponent<UserBase>().TurnInvisible();

                GetComponent<UserBase>().Despawn();

                GetComponent<UserBase>().enabled = false;
                _finalFloor.NewUserInTheFloor();
            });

            t.SetEase(Ease.Linear);

            rb.isKinematic = false;

            GetComponent<Animator>().SetBool("Fly", true);

            gameObject.layer = 10;
        }
    }

    private IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject.transform.parent.gameObject);
    }
}
