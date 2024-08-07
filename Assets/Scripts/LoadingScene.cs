using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingPanel;

    private void Update()
    {
        if(GameObject.FindGameObjectWithTag("Player")  != null)
        {
            LoadingPanel.SetActive(false);
        }
    }
}
