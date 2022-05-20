using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public static bool gameIsPaused = false; //������ ������ �ֳ� ������ �Ǵ��ϴ� ����

    //������ ��ü�� realNote�� �� �ȿ� note�� �迭 ���� ������ �����ؾ��Ѵ�.
    //realNote�� ����� ������Ʈ���� box colider������Ʈ�� �߰��ϰ�is trigger�� üũ���־���Ѵ�.
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
