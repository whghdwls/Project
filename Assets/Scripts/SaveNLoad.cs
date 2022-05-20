using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable] //�����͸� ����ȭ���ִ� ��ɾ�->����ȭ�ϸ� ���� ��ġ�� �а� ���Ⱑ ��������.
public class SaveData  //�÷��̾��� ��ġ�� ����ϴ� Ŭ����
{
    public Vector3 playerPos; //�÷��̾��� ��ġ
    public Vector3 playerRot; //�÷��̾��� �����̼�
}

public class SaveNLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt"; //������ ���� �̸�

    private PlayerContorl thePlayer;

    // Start is called before the first frame update
    void Start()
    { 
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/"; //������ ������ ���丮 ��ġ

        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) //������ ���丮�� ���� ���
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); //������ ���丮 ����
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //������ ���ؼ� json�� Ȱ��
    public void SaveData()
    {
        thePlayer = FindObjectOfType<PlayerContorl>(); //<>�ȿ� ��ũ��Ʈ�� ������ ��ũ��Ʈ�� ã�� ��ɾ�

        saveData.playerPos = thePlayer.transform.position;  //saveData Ŭ������ ������ �÷��̾��� ��ġ�� ����
        saveData.playerRot = thePlayer.transform.eulerAngles; //saveData Ŭ������ ������ �÷��̾��� �����̼ǰ� ����

        //������ ���� json�� ���
        string json = JsonUtility.ToJson(saveData); //json������ ����ȭ�Ͽ� ����

        //json -> ��� string(�ؽ�Ʈ)���� ����
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json); //json�� �̿��Ͽ� �μ�1�� ��ġ�� �����ϰڴٴ� �ǹ�(�������� ������ ����)
        Debug.Log("���� �Ϸ�");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);  //���Ͽ� �ִ� ��� �ؽ�Ʈ�� �о�´�
            saveData = JsonUtility.FromJson<SaveData>(loadJson); //����� json������ �����´�.

            thePlayer = FindObjectOfType<PlayerContorl>();

            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;

            Debug.Log("�ε� �Ϸ�");
        }
        else
        {
            Debug.Log("���̺� ������ �����ϴ�.");
        }
    }
}
