using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable] //데이터를 직렬화해주는 명령어->직렬화하면 저장 장치에 읽고 쓰기가 쉬워진다.
public class SaveData  //플레이어의 위치를 기억하는 클래스
{
    public Vector3 playerPos; //플레이어의 위치
    public Vector3 playerRot; //플레이어의 로테이션
}

public class SaveNLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt"; //저장할 파일 이름

    private PlayerContorl thePlayer;

    // Start is called before the first frame update
    void Start()
    { 
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/"; //저장할 파일의 디렉토리 위치

        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) //저장할 디렉토리가 없는 경우
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); //저장할 디렉토리 생성
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //저장을 위해서 json을 활용
    public void SaveData()
    {
        thePlayer = FindObjectOfType<PlayerContorl>(); //<>안에 스크립트와 동일한 스크립트를 찾는 명령어

        saveData.playerPos = thePlayer.transform.position;  //saveData 클래스의 변수에 플레이어의 위치값 저장
        saveData.playerRot = thePlayer.transform.eulerAngles; //saveData 클래스의 변수에 플레이어의 로테이션값 저장

        //저장을 위해 json을 사용
        string json = JsonUtility.ToJson(saveData); //json파일을 직렬화하여 저장

        //json -> 모든 string(텍스트)들을 저장
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json); //json을 이용하여 인수1의 위치에 저장하겠다는 의미(물리적인 파일을 통해)
        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);  //파일에 있는 모든 텍스트를 읽어온다
            saveData = JsonUtility.FromJson<SaveData>(loadJson); //저장된 json파일을 가져온다.

            thePlayer = FindObjectOfType<PlayerContorl>();

            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;

            Debug.Log("로드 완료");
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다.");
        }
    }
}
