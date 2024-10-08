﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Fusion;
using Fusion.Sockets;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.Diagnostics;
using static Unity.Collections.Unicode;
using System.Linq;

public class CharacterSelection : SimulationBehaviour
{

    public CharacterData[] Characters;
    public CharacterData CurrentData;
    public GameObject PlayerPrefab;
    //
    // Player 1 UI elements

    [Networked] public Image CharImagePlayer1 { get; private set; }
    [Networked] public Image WeaponImagePlayer1 { get; private set; }
    [Networked] public Image PassiveSkillPlayer1 { get; private set; }
    [Networked] public Image ActiveSkillPlayer1 { get; private set; }

    // Player 2 UI elements
    [Networked] public Image CharImagePlayer2 { get; private set; }
    [Networked] public Image WeaponImagePlayer2 { get; private set; }
    [Networked] public Image PassiveSkillPlayer2 { get; private set; }
    [Networked] public Image ActiveSkillPlayer2 { get; private set; }
    public NetworkRunnerManager _networkRunnerManager;
    int playerID;

    private void Start()
    {

        _networkRunnerManager.StartGame(GameMode.Shared, "1");

    }
    public void ChooseChar(int index)
    {
        
        if (Characters == null || Characters.Length == 0)
        {
            Debug.LogError("Characters array is null or empty.");
            return;
        }

        if (index < 0 || index >= Characters.Length)
        {
            Debug.LogError("Index out of bounds: " + index);
            return;
        }

        CurrentData = Characters[index];
        if (CurrentData == null)
        {
            Debug.LogError("Character data at index " + index + " is null.");
            return;
        }

        if (PlayerPrefab == null)
        {
            Debug.LogError("PlayerPrefab is null.");
            return;
        }

        Player playerComponent = PlayerPrefab.GetComponent<Player>();
        if (playerComponent == null)
        {
            Debug.LogError("Player component not found on PlayerPrefab.");
            return;
        }

        playerComponent.Character = CurrentData;

        if (GameObject.FindGameObjectWithTag("nr").GetComponent<NetworkRunner>().IsSharedModeMasterClient)
        {
            UpdateUIForPlayer1();

        }else
        {
            UpdateUIForPlayer2();

        }
        
    }
    private void UpdateUIForPlayer1()
    {
        if (CharImagePlayer1 != null)
        {
            CharImagePlayer1.sprite = CurrentData.CharacterImage;
        }
        if (WeaponImagePlayer1 != null)
        {
            WeaponImagePlayer1.sprite = CurrentData.WeaponImage;
        }
        if (PassiveSkillPlayer1 != null)
        {
            PassiveSkillPlayer1.sprite = CurrentData._passiveSkill;
        }
        if (ActiveSkillPlayer1 != null)
        {
            ActiveSkillPlayer1.sprite = CurrentData._activeSkill;
        }
    }

    private void UpdateUIForPlayer2()
    {
        if (CharImagePlayer2 != null)
        {
            CharImagePlayer2.sprite = CurrentData.CharacterImage;
        }
        if (WeaponImagePlayer2 != null)
        {
            WeaponImagePlayer2.sprite = CurrentData.WeaponImage;
        }
        if (PassiveSkillPlayer2 != null)
        {
            PassiveSkillPlayer2.sprite = CurrentData._passiveSkill;
        }
        if (ActiveSkillPlayer2 != null)
        {
            ActiveSkillPlayer2.sprite = CurrentData._activeSkill;
        }
    }
    public void ChangeScene()
    {
        GameObject.FindGameObjectWithTag("nr").GetComponent<NetworkRunner>().LoadScene("SampleScene");
    }
}
    