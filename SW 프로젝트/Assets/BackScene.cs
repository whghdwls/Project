using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackScene : MonoBehaviour
{
  public void BackBtn()
  {
      SceneManager.LoadScene("Startmenu");

  }
}
