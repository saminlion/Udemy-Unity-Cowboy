﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class TimeOut : MonoBehaviourPun
{

    private float idleTime = 10000f;
    private float timer = 5;

    public GameObject TimeOutUI;
    public Text TimeOutUI_Text;

    private bool TimeOver = false;

  
    void Update()
    {

       // if (photonView.IsMine) { 
        if (!TimeOver)
        {
            if (Input.anyKey)
            {
                idleTime = 10;
            }
            idleTime -= Time.deltaTime;

            if (idleTime <= 0)
            {
                playerNotMoving();
            }

            if (TimeOutUI.activeSelf)
            {
                timer -= Time.deltaTime;
                TimeOutUI_Text.text = "Disconnecting in :" + timer.ToString("F0");
                if (timer <= 0)
                {
                    TimeOver = true;
                }
                else if (timer > 0 && Input.anyKey)
                {
                    idleTime = 10;
                    timer = 5;
                    TimeOutUI.SetActive(false);
                }
            }
        }
        else {
            //leave the room and go back to mainmenu
            leaveGame();
        }
   //  }
    }
    public void playerNotMoving()
    {
        TimeOutUI.SetActive(true);
    }
    void leaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
}
