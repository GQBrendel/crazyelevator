using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICcolorsChanger : MonoBehaviour
{
    [SerializeField] private List<UserBase> _users;
    [SerializeField] private List<Material> _colors;
    [SerializeField] private Image[] _images;

    public void UserEnteredTheElevator(UserBase user)
    {
        _users.Add(user);
        UpdateUI();
    }
    public void UserExitedElevator(UserBase user)
    {
        _users.Remove(user);
        UpdateUI();
    }
    private void UpdateUI()
    {
        for(int i = 0; i < _users.Count; i++)
        {
            _images[i].material = _colors[_users[i].FinalFloor];
        }
        if(_users.Count == 3)
        {
            _images[3].material = null;
        }
        if (_users.Count == 2)
        {
            _images[3].material = null;
            _images[2].material = null;
        }
        if (_users.Count == 1)
        {
            _images[3].material = null;
            _images[2].material = null;
            _images[1].material = null;
        }
        if (_users.Count == 0)
        {
            _images[3].material = null;
            _images[2].material = null;
            _images[1].material = null;
            _images[0].material = null;
        }
    }
}
