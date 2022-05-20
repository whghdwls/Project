using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false; //������ ������ �ֳ� ������ �Ǵ��ϴ� ����

    [SerializeField]
    private GameObject pauseMenuCanvas;

    [SerializeField]
    private SaveNLoad theSaveNLoad;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void ClickSetting()
    {

    }

    public void ClickMenu()
    {

    }

    public void ClickSave()
    {
        Debug.Log("Save");
        theSaveNLoad.SaveData(); //SaveNLoad ��ũ��Ʈ�� SaveData()�Լ��� �ҷ��� ��ư�� ������ ������ �ϵ��� �����.
    }

    public void ClickLoad()
    {
        Debug.Log("Load");
        theSaveNLoad.LoadData();
    }
}
