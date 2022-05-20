using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오브젝트(cube)에 적용하는 코드
//오브젝트의 box colider에 is trigger를 체크한다.
//오브젝트의 inspector창에서 transform밑에 mesh filter에서 mesh를 None으로 바꾸어야 투명해진다.
public class TriggerOpen : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObject; //알맞는 ui를 넣는다.
    private bool isPaused = false;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag=="Player")
        {
            if(isPaused == false)
            {
                gameObject.SetActive(true);
                Time.timeScale = 0f;
                isPaused = true;
                GetComponent<BoxCollider>().isTrigger = true;
            }
        }
    }
}
