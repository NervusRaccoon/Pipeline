using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject winPanel;
    public SpawnBoard spawnBoard;
    public PipesList pipesList;
    public Transform pipesParent;
    public Text timerText;
    public Text gamePanelLevelTextName;
    private float timeInSeconds = 0;
        
    private const string levelTextName = "LevelText";
    private const string timerStoppedName = "TimerText";
    private const string levelText = "LEVEL ";
    private List<string> tags = new List<string>{"Flat", "Angle"};

    private void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        timerText.text = "00:00";
        timeInSeconds = 0;
        gamePanelLevelTextName.text = levelText + spawnBoard.levelNumber.ToString();
        winPanel.SetActive(false);
    }

    private void Update()
    {
        if (!ProgressCheck.winDiscovered)
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

    public void WinPanelOpened()
    {
        winPanel.SetActive(true);
        foreach(Transform child in winPanel.transform)
        {
            if (child.gameObject.name == levelTextName)
                child.gameObject.GetComponent<Text>().text = levelText + spawnBoard.levelNumber.ToString();
            else if (child.gameObject.name == timerStoppedName)
                child.gameObject.GetComponent<Text>().text = timerText.text;
        }
    }

    public void CloseWinPanel()
    {
        winPanel.SetActive(false);
        spawnBoard.StartSpawnBoard();
        ProgressCheck.winDiscovered = false;
        StartLevel();
    }

    public void PressedHelpButton(int count)
    {
        count++;
        int randomID = Random.Range(0, pipesParent.childCount-1);
        GameObject randomPipe = pipesParent.GetChild(randomID).gameObject;
        bool rotated = false;
        if (tags.Contains(randomPipe.tag))
            foreach(PipeData pipe in pipesList.list)
                if (pipe.id == randomPipe.name && !rotated)
                {
                    float prevRotation = randomPipe.transform.eulerAngles.z;
                    
                    if (randomPipe.tag == tags[0])
                        if ((pipe.rotZ == 0 && prevRotation != 180 && prevRotation != -180 && prevRotation != 0) || 
                            (pipe.rotZ == -90 && prevRotation != 270 && prevRotation != -270 && prevRotation != 90 && prevRotation != -90))
                            rotated = true;

                    if (randomPipe.tag == tags[1])
                        if ((pipe.rotZ == -90 && prevRotation != 270 && prevRotation != -90) || 
                            (pipe.rotZ == -180 && prevRotation != -180 && prevRotation != 180) ||
                            (pipe.rotZ == -270 && prevRotation != -270 && prevRotation != 90) ||
                            (pipe.rotZ == 0 && prevRotation != 0))
                            rotated = true;

                    if (rotated)
                    {
                        randomPipe.transform.rotation = Quaternion.Euler(0, 0, pipe.rotZ);
                        pipesParent.gameObject.GetComponent<ProgressCheck>().CheckWin(randomPipe, prevRotation);
                    }
                    Debug.Log(prevRotation);                    
                }
        if (!rotated)
            PressedHelpButton(count);
        /*if (count <= 5)
            Debug.Log("Тут нечего поворачивать"); */
    }

}
