using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndText : MonoBehaviour
{
    public void retryGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
