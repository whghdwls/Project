using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animation_manager : MonoBehaviour
{

    Ray ray;
    RaycastHit hit;
    public Animator anim_door; // 3D object 계속 추가 할 것. 
    bool game_ongoing = true; // 현재 게임 진행중 
    string door_clip_name = "open";

    void Start()
    {
        anim_door = anim_door.GetComponent<Animator>();
    }

     void Update()
     {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                
                if (hit.collider != null)
                {
                    // **************  3D object 마우스클릭을 통해 애니메이션을 변경하는 경우 **************
                    switch(hit.collider.name){
                        case "door_container":
                            if(door_clip_name == "open"){ // anim_clip_name 의 init 값은 open
                                anim_door.Play("door_open");
                                door_clip_name = "close";
                            }else{ // 한번 door_open 하고 나면 close로 anim_clip_name이 변경됨 => door_close 완료후, 다시 open으로 변경 
                                anim_door.Play("door_close");
                                door_clip_name = "open";
                            }
                            break;
                        // ex) 다른 오브젝트 애니메이션 추가시 상요 
                    }

                    // *******************************************************************************
                }
            }
        }
         // 방탈출 성공시 => game_ongoing = false => key 등장 및 door이 오픈되어야 함. 
        // if(game_ongoing==false) 
        // {
        //     EscapeSuccess() 
        // }

     }
     public void EscapeSuccess(){
        // => key animation(아래에서 위로 올라오는) 추가 => 키로 문여는 사운드 재생 => door_open 애니메이션 실행 (anim_door.Play("door_open"))
     }



}
