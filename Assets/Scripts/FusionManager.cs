using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
}

public class FusionManager : MonoBehaviour
{
    [SerializeField] private NetworkRunner NetworkRunnerPref;
    

    [SerializeField] private TMP_InputField InputField;
    [SerializeField] private GameObject Menu_01;
    [SerializeField] private GameObject Menu_02;

    public NetworkRunner _runner;
    public Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private void Start()
    {
        Menu_01.SetActive(true);
        Menu_02.SetActive(false);
    }

    

    async void StartGame(GameMode mode,string roomName)
    {
        _runner = Instantiate(NetworkRunnerPref);
        _runner.ProvideInput = true;
        _runner.GetComponent<GameRunnerCallback>().Init(this);

        StartGameArgs config = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.GetComponent<NetworkSceneManagerDefault>()
        };
        await _runner.StartGame(config);

        Menu_01.SetActive(false);
        Menu_02.SetActive(true);
    }

    public void StartAGame()
    {
        StartGame(GameMode.Host, InputField.text);
    }

    public void JoinAGame()
    {
        StartGame(GameMode.Client, InputField.text);
    }

    public void ExitGame()
    {
        _runner.Shutdown();
        Menu_01.SetActive(true);
        Menu_02.SetActive(false);
    }
}
