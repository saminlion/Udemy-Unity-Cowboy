using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CowBoy : MonoBehaviourPun {

    public float MoveSpeed = 5;
    public GameObject playerCam;
    public SpriteRenderer sprite;
    public Animator anim;
    public PhotonView photonview;
    public GameObject bulletPrefab;
    public GameObject spawnPoint;

    private bool allowMoving = true;

	// Use this for initialization
	void Awake () {
        if (photonView.IsMine)
        { 
         playerCam.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            checkInputs();
        }
    }

    private void checkInputs()
    {
        if (allowMoving)
        {
            var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
            transform.position += movement * MoveSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.F) && !anim.GetBool("IsMove"))
        {
            Shot();
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            anim.SetBool("IsShot", false);
            allowMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.D) && !anim.GetBool("IsShot"))
        {
            photonview.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
            anim.SetBool("IsMove", true);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("IsMove", false);
        }

        if (Input.GetKeyDown(KeyCode.A) && !anim.GetBool("IsShot"))
        {
            anim.SetBool("IsMove", true);
            photonview.RPC("FlipSprite_Left", RpcTarget.AllBuffered);
        }

        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("IsMove", false);
        }
    }

    private void Shot()
    {
        anim.SetBool("IsShot", true);
        allowMoving = false;
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name,
            new Vector2(bulletPrefab.transform.position.x, bulletPrefab.transform.position.y), Quaternion.identity, 0);

        if (sprite.flipX)
        {
            bullet.GetComponent<PhotonView>().RPC("ChangeDirection", RpcTarget.AllBuffered);
        }
        else
        {

        }
    }

    [PunRPC]
    private void FlipSprite_Right()
    {
        sprite.flipX = false;
    }

    [PunRPC]
    private void FlipSprite_Left()
    {
        sprite.flipX = true;
    }
}
