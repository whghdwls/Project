using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public Transform Target;

    void OnTriggerEnter(Collider col)  // Ʈ���ſ� �浹�� �Ǿ��� ���� �� �Լ��� ȣ���Ѵ�.
    {
        if (col.gameObject.tag == "Player")
        {
            col.transform.position = Target.position; // �ε��� ��ü�� Ÿ���� ��ġ�� ������.

        }
    }

}
