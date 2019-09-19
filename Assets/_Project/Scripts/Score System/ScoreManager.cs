using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public delegate void UserScoredHandler();
    public UserScoredHandler OnUserScored;

    public Text ScoreText;
    public Text ScoreTextFinal;
    public int userScoreValue = 100;
    public int CurrentScore;

    private List<UserBase> _userList0 = new List<UserBase>();
    private List<UserBase> _userList1 = new List<UserBase>();
    private List<UserBase> _userList2 = new List<UserBase>();
    private List<UserBase> _userList3 = new List<UserBase>();
    private List<UserBase> _userList4 = new List<UserBase>();
    private List<UserBase> _userList5 = new List<UserBase>();
    private List<List<UserBase>> _listOfLists = new List<List<UserBase>>();

    [SerializeField] private Text _combo;

    private void Start()
    {
        CurrentScore = 0;
        _combo.gameObject.SetActive(false);
        _listOfLists.Add(_userList0);
        _listOfLists.Add(_userList1);
        _listOfLists.Add(_userList2);
        _listOfLists.Add(_userList3);
        _listOfLists.Add(_userList4);
        _listOfLists.Add(_userList5);
    }
    public void AddScore(UserBase user)
    {
        switch (user.FinalFloor)
        {
            case 0:
                _userList0.Add(user);
                break;
            case 1:
                _userList1.Add(user);
                break;
            case 2:
                _userList2.Add(user);
                break;
            case 3:
                _userList3.Add(user);
                break;
            case 4:
                _userList4.Add(user);
                break;
            case 5:
                _userList5.Add(user);
                break;

        }
        StartCoroutine(ScoreTimer());
    }
    private void ComboAnimation()
    {
        _combo.gameObject.SetActive(true);
        StartCoroutine(Disable());
    }
    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(1.5f);
        _combo.gameObject.SetActive(false);
    }
    private IEnumerator ScoreTimer()
    {
        yield return new WaitForSeconds(1f);

        foreach (var list in _listOfLists)
        {
            if(list.Count == 4)
            {
                _combo.text = "x4!!!!";
                ComboAnimation();
                CurrentScore += userScoreValue*8;
                break;
            }
            else if (list.Count == 3)
            {
                _combo.text = "x3!!!";
                ComboAnimation();
                CurrentScore += userScoreValue * 6;
                break;
            }
            else if (list.Count == 2)
            {
                _combo.text = "x2!!";
                ComboAnimation();
                CurrentScore += userScoreValue * 4;
                break;
            }
            else if (list.Count == 1)
            {
                CurrentScore += userScoreValue;
                break;
            }
        }
        foreach (var list in _listOfLists)
        {
            list.Clear();
        }
        ScoreText.text = CurrentScore.ToString();
        ScoreTextFinal.text = CurrentScore.ToString();
        OnUserScored?.Invoke();
    }
}
