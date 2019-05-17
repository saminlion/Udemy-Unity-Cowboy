using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class ChatManager : MonoBehaviourPun,IPunObservable
{

    public PhotonView photonView;
    public GameObject BubbleSpeech;
    public Text ChatText;

    public CowBoy player;
    InputField ChatInput;
    private bool DisableSend;

    void Awake()
    {
        ChatInput = GameObject.Find("ChatInputField").GetComponent<InputField>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (ChatInput.isFocused)
            {
                player.DisableInputs = true;
            }
            else {
                player.DisableInputs = false;

            }

            if (!DisableSend && ChatInput.isFocused)
            {
                if (ChatInput.text != "" && ChatInput.text.Length > 1 && Input.GetKeyDown(KeyCode.Space))
                {
                    photonView.RPC("SendMsg", RpcTarget.AllBuffered, ChatInput.text);
                    BubbleSpeech.SetActive(true);
                    ChatInput.text = "";
                    DisableSend = true;

                }
            }
        }
    }


    [PunRPC]
    void SendMsg(string msg)
    {
        ChatText.text = msg;
        StartCoroutine(hideBubbleSpeech());
    }
    IEnumerator hideBubbleSpeech()
    {
        yield return new WaitForSeconds(3);
        BubbleSpeech.SetActive(false);
        DisableSend = false;

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(BubbleSpeech.activeSelf);
            //
        }
        else if (stream.IsReading) {
            BubbleSpeech.SetActive( (bool)stream.ReceiveNext());
            //
        }
    }
}
