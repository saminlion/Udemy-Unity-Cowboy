using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviourPun {

    public Image fillImage;
    public float health = 1;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public BoxCollider2D collider;
    public GameObject playerCanvas;

    public CowBoy playerScript;
    public GameObject KillGotKilledText;

    public void CheckHealth()
    {
        if (photonView.IsMine && health <= 0)
        {
            GameManager.instance.EnableRespawn();
            playerScript.DisableInputs = true;
            this.GetComponent<PhotonView>().RPC("death", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void death()
    {
        rb.gravityScale = 0;
        collider.enabled = false;
        sr.enabled = false;
        playerCanvas.SetActive(false);
    }
    [PunRPC]
    public void Revive()
    {
        rb.gravityScale = 1;
        collider.enabled = true;
        sr.enabled = true;
        playerCanvas.SetActive(true);
        fillImage.fillAmount = 1;
        health = 1;
    }
    public void EnableInputs()
    {
        playerScript.DisableInputs = false;
    }
    [PunRPC]
    public void HealthUpdate(float damage)
    {

        fillImage.fillAmount -= damage;
        health = fillImage.fillAmount;
        CheckHealth();
    }

    [PunRPC]
    void YouGotKilledBy(string name)
    {
        GameObject go = Instantiate(KillGotKilledText, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(GameManager.instance.KillGotKilledFeedBox.transform,false);
        go.GetComponent<Text>().text = "You Got Killed by : " + name;
        go.GetComponent<Text>().color = Color.red;
    }

    [PunRPC]
    void YouKilled(string name)
    {
        GameObject go = Instantiate(KillGotKilledText, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(GameManager.instance.KillGotKilledFeedBox.transform, false);
        go.GetComponent<Text>().text = "You Killed : " + name;
        go.GetComponent<Text>().color = Color.green;
    }
}
