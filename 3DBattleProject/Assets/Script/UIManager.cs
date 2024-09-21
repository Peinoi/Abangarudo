using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviourPunCallbacks
{
    public static UIManager instance = null;

    public PhotonView pv;

    [Header("UI")]
    public TextMeshProUGUI bulletText;
    public TextMeshProUGUI reloadItem;
    public int reloadValue = 0;
    //public Slider hp;
    public TextMeshProUGUI hp_Txt;
    public TextMeshProUGUI hitPoint;
    [Header("�� ����")]
    public Transform target_ObjTr;
    public TextMeshProUGUI team_A;
    public int teamA_Point = 0;
    public TextMeshProUGUI team_B;
    public int teamB_Point = 0;
    [Header("���â")]
    public GameObject result_Box;
    public Text result_Txt;
    public Text result_Txt2;
    public Button end;
    public Button restart;
    [Header("����")]
    public GameObject open_Help;

    public GameObject uiBox;

    private void Awake()
    {
        instance = this;
  
       // hp.value = 1000;
        hp_Txt.text = "1000";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }
    public void StartGame()
    {
        uiBox.SetActive(true);
    }

    [PunRPC]
    public void PointCheck()
    {
        teamA_Point = 0;
        teamB_Point = 0;

        for (int i = 0; i < target_ObjTr.childCount; i++)
        {
            Renderer obj_Color = target_ObjTr.GetChild(i).GetComponent<Renderer>();
            if (obj_Color.material.color == Color.red)
            {
                teamA_Point += 1;
            }
            else if (obj_Color.material.color == Color.blue)
            {
                teamB_Point += 1;
            }
        }

        // ��� Ŭ���̾�Ʈ���� ���� ������Ʈ
        pv.RPC("UpdateScore", RpcTarget.All, teamA_Point, teamB_Point);

        if (teamA_Point >= 13)
        {
            Debug.Log("TeamA Win");

            // ��� �÷��̾ ��ȸ�ϸ� ���� ������Ʈ
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                if (player.Key == photonView.OwnerActorNr) // ���� �÷��̾ A���̶��
                {
                    UIManager.instance.pv.RPC("Result", player.Value, 0); // WIN
                }
                else // B�� �÷��̾�
                {
                    UIManager.instance.pv.RPC("Result", player.Value, 3); // FAIL
                }
            }
        }
        else if (teamB_Point >= 13)
        {
            Debug.Log("TeamB Win");

            // ��� �÷��̾ ��ȸ�ϸ� ���� ������Ʈ
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                if (player.Key == photonView.OwnerActorNr) // ���� �÷��̾ B���̶��
                {
                    UIManager.instance.pv.RPC("Result", player.Value, 0); // WIN
                }
                else // A�� �÷��̾�
                {
                    UIManager.instance.pv.RPC("Result", player.Value, 3); // FAIL
                }
            }
        }

    }
    [PunRPC]
    public void Result(int status)
    {
        result_Box.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        if (status == 0) // WIN
        {
            result_Txt.text = "You Win!";
        }
        else if (status == 3) // FAIL
        {
            result_Txt.text = "You Lose!";
        }
    }





    [PunRPC]
    public void UpdateScore(int teamAPoints, int teamBPoints)
    {
        teamA_Point = teamAPoints;
        teamB_Point = teamBPoints;

        team_A.text = "Red = " + teamA_Point.ToString();
        team_B.text = "Blue = " + teamB_Point.ToString();
    }

   
    public void ReGame()
    {
        SceneManager.LoadScene(0);
    }
    public void GameEnd()
    {
        Application.Quit();
    }
    public void OpenHelp()
    {
        open_Help.SetActive(true);
    }
    public void CloseHelp()
    {
        open_Help.SetActive(false);
    }
}
