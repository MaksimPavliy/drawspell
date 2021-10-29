using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour
{
    [SerializeField] private GameObject restartMenu;
    [SerializeField] private GameObject lean;

    public void OnPlayPressed()
    {
        SceneManager.LoadScene("Runner", LoadSceneMode.Single);
    }

    public void OnRestartPressed()
    {
        restartMenu.SetActive(false);
        SceneManager.LoadScene("Runner", LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    public void SetRestartMenuActive(bool value)
    {
        Time.timeScale = 0;
        restartMenu.SetActive(value);
    }

    public void SetLeanTouchActive(bool value)
    {
        lean.SetActive(value);
    }
}
