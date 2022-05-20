using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������Ʈ(cube)�� �����ϴ� �ڵ�
//������Ʈ�� box colider�� is trigger�� üũ�Ѵ�.
//������Ʈ�� inspectorâ���� transform�ؿ� mesh filter���� mesh�� None���� �ٲپ�� ����������.
public class TriggerOpen : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObject; //�˸´� ui�� �ִ´�.
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
