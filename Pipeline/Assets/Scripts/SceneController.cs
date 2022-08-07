using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject winPanel;
    public SpawnBoard spawnBoard;
        
    private const string levelTextName = "LevelText";
    private const string levelText = "LEVEL ";

    private void Start()
    {
        winPanel.SetActive(false);
    }

    public void WinPanelOpened()
    {
        winPanel.SetActive(true);
        foreach(Transform child in winPanel.transform)
        {
            if (child.gameObject.name == levelTextName)
                child.gameObject.GetComponent<Text>().text = levelText + spawnBoard.levelNumber.ToString();
        }
    }

    public void CloseWinPanel()
    {
        winPanel.SetActive(false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        spawnBoard.StartSpawnBoard();
        ProgressCheck.winDiscovered = false;
    }
}
