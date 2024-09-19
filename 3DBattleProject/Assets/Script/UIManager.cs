using Photon.Pun;
using TMPro;
using UnityEngine;

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
    [Header("팀 점수")]
    public Transform target_ObjTr;
    public TextMeshProUGUI team_A;
    public int teamA_Point = 0;
    public TextMeshProUGUI team_B;
    public int teamB_Point = 0;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
       // hp.value = 1000;
        hp_Txt.text = "1000";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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

        // 모든 클라이언트에서 점수 업데이트
        pv.RPC("UpdateScore", RpcTarget.All, teamA_Point, teamB_Point);

        if (teamA_Point >= 15)
        {
            Debug.Log("TeamA Win");
        }
        else if (teamB_Point >= 15)
        {
            Debug.Log("TeamB Win");
        }
    }

    [PunRPC]
    public void UpdateScore(int teamAPoints, int teamBPoints)
    {
        teamA_Point = teamAPoints;
        teamB_Point = teamBPoints;

        team_A.text = "Team Red = " + teamA_Point.ToString();
        team_B.text = "Team Blue = " + teamB_Point.ToString();
    }
}
