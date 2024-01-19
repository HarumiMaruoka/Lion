using System;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
    [SerializeField]
    private Button _pauseButton;
    [SerializeField]
    private Button _resumeButton;
    [SerializeField]
    private Text _pauseCountText;

    [SerializeField]
    private Button SpeedChangeButton1x;
    [SerializeField]
    private Button SpeedChangeButton2x;
    [SerializeField]
    private Button SpeedChangeButton3x;

    private void Start()
    {
        _pauseButton.onClick.AddListener(PauseManager.Instance.Pause);
        _resumeButton.onClick.AddListener(PauseManager.Instance.Resume);

        SpeedChangeButton1x.onClick.AddListener(() => GameSpeedManager.Instance.TimeScale = 1f);
        SpeedChangeButton2x.onClick.AddListener(() => GameSpeedManager.Instance.TimeScale = 2f);
        SpeedChangeButton3x.onClick.AddListener(() => GameSpeedManager.Instance.TimeScale = 3f);
    }

    private void Update()
    {
        _pauseCountText.text = "Pause Count: " + PauseManager.Instance.PauseCount.ToString();
    }
}