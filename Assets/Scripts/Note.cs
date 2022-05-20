using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public static bool gameIsPaused = false; //게임이 정지해 있나 없나를 판단하는 변수

    //누르는 주체인 realNote와 그 안에 note는 배열 내에 순서가 동일해야한다.
    //realNote로 사용할 오브젝트에는 box colider컴포넌트를 추가하고is trigger를 체크해주어야한다.
    [SerializeField]
    private GameObject[] note;

    [SerializeField]
    private Collider[] realNote;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<note.Length; i++)
        {
            note[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Resume();
            }
        }
        else
        {        
            Click();
        }
    }

    private void Resume()
    {
        for (int i = 0; i < note.Length; i++)
        { 
            note[i].SetActive(false);
            Time.timeScale = 1f;
            gameIsPaused = false;
        }
    }

    private void Click()
    {
        if(Physics.Raycast(transform.position,transform.forward,out hit, 2f))
        {
            Collider isNote = hit.transform.GetComponent<Collider>();
            for(int i=0; i<realNote.Length; i++)
            {
                if (isNote == realNote[i] && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    note[i].SetActive(true);
                    Time.timeScale = 0f;
                    gameIsPaused = true;
                }
            }
            
        }
    }
}
