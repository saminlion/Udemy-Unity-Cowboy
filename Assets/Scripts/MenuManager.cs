using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks {

    [SerializeField]
    private GameObject UserNameScreen, ConnectScreen;

    [SerializeField]
    private GameObject CreateUserNameButton;

    [SerializeField]
    private InputField UserNameInput, CreateRoomInput, JoinRoomInput;


    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to masterr!!!!!");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to Lobby!!!");
        UserNameScreen.SetActive(true);
    }
    public override void OnJoinedRoom()
    {
        // play game scene
        PhotonNetwork.LoadLevel(1);
    }
    #region UIMethods


    public void OnClick_CreateNameBtn()
    {
       
        PhotonNetwork.NickName = UserNameInput.text;
        UserNameScreen.SetActive(false);
        ConnectScreen.SetActive(true);
    }

    public void OnNameField_Changed()
    {
        if (UserNameInput.text.Length >= 2)
        {
            CreateUserNameButton.SetActive(true);
        }
        else {
            CreateUserNameButton.SetActive(false);
        }
    }

    public void Onclick_JoinRoom()
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(JoinRoomInput.text, ro, TypedLobby.Default);

    }
    public void Onclick_CreateRoom()
    {
        PhotonNetwork.CreateRoom(CreateRoomInput.text, new RoomOptions { MaxPlayers = 4 }, null);
 }
    #endregion
}
