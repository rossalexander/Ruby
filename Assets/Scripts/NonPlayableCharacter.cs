using UnityEngine;

public class NonPlayableCharacter : MonoBehaviour
{
    [SerializeField] private float _displayTime = 4.0f;
    private float _timerDisplay;
    [SerializeField] private GameObject dialogBox;

    private void Start()
    {
        dialogBox.SetActive(false);
        _timerDisplay = -1.0f;
    }

    private void Update()
    {
        if (_timerDisplay >= 0)
        {
            _timerDisplay -= Time.deltaTime;

            if (_timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        _timerDisplay = _displayTime;
        dialogBox.SetActive(true);
    }
}