using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameEndUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _playerWinText;
    public TextMeshProUGUI playerWinText { get { return _playerWinText; } }
    public Button restartButton;
    private void Awake()
    {
        // gameObject.SetActive(false);
    }
}
