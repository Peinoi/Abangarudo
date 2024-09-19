using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;

public class TargetObj : MonoBehaviour
{
    Renderer obj_Color;

    private void Start()
    {
        obj_Color = gameObject.GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            PhotonView photonView = PhotonView.Get(this);

                // Color를 RGBA로 변환해서 전송
                photonView.RPC("ChangeColor", RpcTarget.All, 1f, 0f, 0f); // 빨간색
            
        }
        else if (collision.gameObject.CompareTag("BulletB"))
        {
            PhotonView photonView = PhotonView.Get(this);
           
                // Color를 RGBA로 변환해서 전송
                photonView.RPC("ChangeColor", RpcTarget.All, 0f, 0f, 1f); // 파란색
            
        }
    }

    [PunRPC]
    public void ChangeColor(float r, float g, float b)
    {
        Color color = new Color(r, g, b);
        obj_Color.material.color = color;
        UIManager.instance.PointCheck();
    }
}
