using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public Transform Target;

    void OnTriggerEnter(Collider col)  // 트리거에 충돌이 되었을 때는 이 함수를 호출한다.
    {
        if (col.gameObject.tag == "Player")
        {
            col.transform.position = Target.position; // 부딪힌 객체를 타겟의 위치로 보낸다.

        }
    }

}
