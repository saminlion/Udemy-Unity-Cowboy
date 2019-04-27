using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public bool movingDirection;

    public float moveSpeed = 8f;

    public float destoryTime = 2f;

    IEnumerator DestoryBullet()
    {
        yield return new WaitForSeconds(destoryTime);
        this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
    }

    private void Update()
    {
        if (!movingDirection)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }

        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
    }

    [PunRPC]
    public void ChangeDirection()
    {
        movingDirection = true;
    }

    [PunRPC]
    void Destroy()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (target != null && (!target.IsMine || target.IsSceneView))
        {
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
        }
    }
}
