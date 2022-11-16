using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameEndUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _playerWinText;
    public TextMeshProUGUI playerWinText { get { return _playerWinText; } }
    private void Awake()
    {
        // gameObject.SetActive(false);
    }
}
