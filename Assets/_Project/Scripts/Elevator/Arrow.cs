using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    Tween _arrowTween;
    [SerializeField] private float _duration;
    [SerializeField] private float _highness = 1;
    [SerializeField] private float _scaleFactor = 1.5f;

    [SerializeField] private Vector3 _topPosition;
    private Vector3 _startPosition;
    private Vector3 _startScale;


    private void Start()
    {
        _startPosition = transform.position;
        _startScale = transform.localScale;

        StartCoroutine(DoTweenAnimationRoutine());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void DoTweenAnimation()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(DoTweenAnimationRoutine());
        }
    }

    private IEnumerator DoTweenAnimationRoutine()
    {
        yield return null;
        Tween moveUp, moveDown, scale, descale;
        Sequence moveSequence = DOTween.Sequence();
        Sequence scaleSequence = DOTween.Sequence();

        _topPosition = new Vector3(transform.position.x, transform.position.y + _highness, transform.position.z);

        moveUp = transform.DOMove(_topPosition, _duration);
        moveDown = transform.DOMove(_startPosition, _duration);
        scale = transform.DOScale(transform.localScale * _scaleFactor, _duration);

        descale = transform.DOScale(_startScale, _duration);

        moveSequence.Append(moveUp);
        moveSequence.Append(moveDown);
        scaleSequence.Append(scale);
        scaleSequence.Append(descale);

        moveSequence.Play().OnComplete(() => scaleSequence.Play().OnComplete(() => DoTweenAnimation()));
    }
}
    

