using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScoreForUI : MonoBehaviour
{
    [SerializeField] private GameObject _user;
    [SerializeField] private float _timeToReachScore = 2f;

    private GameObject _flyDestination;
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
        _flyDestination = GameObject.FindGameObjectWithTag("ScoreBox");
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TestUI")
        {
            timeStamp = Time.time;

            _rootTransform.LookAt(_flyDestination.transform);
            Tween t = _rootTransform.DOMove(_flyDestination.transform.position, 2f).OnComplete(() => 
            {
                _scoreManager.AddScore(GetComponent<UserBase>());
                _user.SetActive(false);
            });

            t.SetEase(Ease.Linear);

            rb.isKinematic = false;
            GetComponent<Animator>().SetBool("Fly", true);

            GetComponent<UserBase>().enabled = false;
            gameObject.layer = 10;
        }

        if (other.CompareTag("ScoreBox"))
        {
            _scoreManager.AddScore(GetComponent<UserBase>());
            _user.SetActive(false);
        }
    }

    private IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
