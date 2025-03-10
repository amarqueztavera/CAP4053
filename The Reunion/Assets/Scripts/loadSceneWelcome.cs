using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class loadSceneWelcome : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("Map", LoadSceneMode.Single);
    }
}
