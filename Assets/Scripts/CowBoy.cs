using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class CowBoy : MonoBehaviourPun {

    public float MoveSpeed = 5;
    public GameObject playerCam;
    public SpriteRenderer sprite;
    public PhotonView photonview;
    public  Animator anim;
    private bool AllowMoving = true;

    public GameObject BulletePrefab;
    public Transform BulleteSpawnPointRight;
    public Transform BulleteSpawnPointleft;

    public Text PlayerName;
    public bool IsGrounded = false;
    public bool DisableInputs = false;
    private Rigidbody2D rb;
    public float jumpForce;

    public string MyName;
    // Use this for initialization
    void Awake ()
    {
        if (photonView.IsMine)
        {
            GameManager.instance.LocalPlayer = this.gameObject;
            playerCam.SetActive(true);
            playerCam.transform.SetParent(null, false);
            PlayerName.text = "You : "+PhotonNetwork.NickName;
            PlayerName.color = Color.green;
            MyName = PhotonNetwork.NickName;
        }
        else
        {
            PlayerName.text = photonview.Owner.NickName;
           
            PlayerName.color = Color.red;

        }

    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine&& !DisableInputs)
        {
            checkInputs();
        }
    }

    private void checkInputs()
    {
        if (AllowMoving)
        { 
        var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += movement * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.RightControl) && anim.GetBool("IsMove") == false)
        {
            shot();
        }
        else if (Input.GetKeyUp(KeyCode.RightControl))
        {
            anim.SetBool("IsShot", false);
            AllowMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            anim.SetBool("IsMove", true);
        }
        if (Input.GetKeyDown(KeyCode.D) && anim.GetBool("IsShot") == false)
        {

            //FlipSprite_Right()
            playerCam.GetComponent<CameraFollow2D>().offset = new Vector3(1.3f, 1.53f, 0);
            photonview.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("IsMove", false);
        }

        if (Input.GetKeyDown(KeyCode.A) && anim.GetBool("IsShot") == false)
        {

            //FlipSprite_Left()
            playerCam.GetComponent<CameraFollow2D>().offset = new Vector3(-1.3f, 1.53f, 0);


            photonview.RPC("FlipSprite_Left", RpcTarget.AllBuffered);

        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("IsMove", false);
        }
    }

    private void shot()
    {

        if (sprite.flipX == false)
        {
            GameObject bullete = PhotonNetwork.Instantiate(BulletePrefab.name, new Vector2(BulleteSpawnPointRight.position.x, BulleteSpawnPointRight.position.y), Quaternion.identity, 0);
            bullete.GetComponent<Bullete>().localPlayerObj = this.gameObject;
        }
        
        if (sprite.flipX == true)
        {
            GameObject bullete = PhotonNetwork.Instantiate(BulletePrefab.name, new Vector2(BulleteSpawnPointleft.position.x, BulleteSpawnPointleft.position.y), Quaternion.identity, 0);
            bullete.GetComponent<Bullete>().localPlayerObj = this.gameObject;

            bullete.GetComponent<PhotonView>().RPC("ChangeDirection", RpcTarget.AllBuffered);
        }
        
        anim.SetBool("IsShot", true);
        AllowMoving = false;

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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            IsGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            IsGrounded = false;
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce * Time.deltaTime));
    }
}
