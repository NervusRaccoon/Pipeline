using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject winPanel;
    public SpawnBoard spawnBoard;
    public Text timerText;
    public Text gamePanelLevelTextName;

    private GameController gameController;
    private float timeInSeconds = 0;    
    private const string levelTextName = "LevelText";
    private const string timerStoppedName = "TimerText";
    private const string levelText = "LEVEL ";

    private void Start()
    {
        gameController = this.GetComponent<GameController>();
        StartLevel();
    }

    public void StartLevel()
    {
        timerText.text = "00:00";
        timeInSeconds = 0;
        gamePanelLevelTextName.text = levelText + (PlayerPrefs.GetInt("levelNumber")+1).ToString();
        winPanel.SetActive(false);
    }

    private void Update()
    {
        if (!GameController.winDiscovered)
        {
            timeInSeconds += Time.deltaTime;

            int secondCount = (int)(timeInSeconds % 60);
            int minuteCount = (int)(timeInSeconds / 60);

            if (minuteCount < 10) timerText.text = "0" + minuteCount.ToString();
            else timerText.text = minuteCount.ToString();

            timerText.text += ":";

            if (secondCount < 10) timerText.text = timerText.text + "0" + secondCount.ToString();
            else timerText.text += secondCount.ToString();
        }
    }

    public IEnumerator WinPanelOpened()
    {
        yield return new WaitForSeconds(1f);
        winPanel.SetActive(true);
        foreach(Transform child in winPanel.transform)
        {
            if (child.gameObject.name == levelTextName)
                child.gameObject.GetComponent<Text>().text = levelText + PlayerPrefs.GetInt("levelNumber").ToString();
            else if (child.gameObject.name == timerStoppedName)
                child.gameObject.GetComponent<Text>().text = timerText.text;
        }
    }

    public void CloseWinPanel()
    {
        GameController.winDiscovered = false;
        spawnBoard.StartSpawnBoard();
        StartLevel();
    }

    public void PressedHelpButton()
    {
        gameController.AutoRotateCell();
    }

}
