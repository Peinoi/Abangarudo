using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // 버전 입력
    private readonly string version = "1.0f";
    // 사용자 아이디 입력
    [SerializeField] private InputField userid;
    [SerializeField] private GameObject Login_Pop;
    [SerializeField] private GameObject Game_UI;
    [SerializeField] private GameObject Lobby_Camer;
    [SerializeField] private TextMeshProUGUI userName;
    private void StartGame()
    {
        DontDestroyOnLoad(this);
        // 같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;
        // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = version;
        // 포톤 서버와 통신 횟수 설정. 초당 30회
        Debug.Log(PhotonNetwork.SendRate);
        // 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }
    // 포톤 서버에 접속 후 호출되는 롤백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby(); //로비 입장
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom(); //랜덤 매치메이킹 기능 제공
    }

    // 랜덤한 룸 입장이 실패했을 경우 호출되는 콜백 함수

    public override void OnJoinRandomFailed(short returnCord, string message) 
    {
        Debug.Log($"JoinRandom Filed {returnCord} : {message}"); // 생성되지 않앗을때 나옴

        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4; // 최대 접속자 수 : 4명
        ro.IsOpen = true; // 룸의 오픈 여부
        ro.IsVisible = true;  // 로비에서 룸 목록에 노출 시킬지 여부 (공개,비공개)

        // 룸 생성
        PhotonNetwork.CreateRoom("My Room", ro);
    }

    // 룸 생성이 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    // 룸에 입장한 후 호출되는 롤백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        // 룸에 접속한 사용자 정보 확인
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName}.{player.Value.ActorNumber}");
            // $ => String.Format()
        }
        int idx_Num = PhotonNetwork.LocalPlayer.ActorNumber;
        // 캐릭터 툴현 정보를 배열에 저장
        Transform[] points = GameObject.Find("Map").GetComponentsInChildren<Transform>();

        if (idx_Num == 1)
        {
           
            int idx = Random.Range(1, 2);
            // 캐릭터를 생성

            PhotonNetwork.Instantiate("PlayerCharacter", points[idx].position, points[idx].rotation, 0);
        }
        else if (idx_Num <= 2)
        {
           
            int idx = Random.Range(3, points.Length);
            // 캐릭터를 생성

            PhotonNetwork.Instantiate("PlayerCharacter 1", points[idx].position, points[idx].rotation, 0);
        }
    }
    public void Checkname()
    {
        // 유저 아이디 할당
        PhotonNetwork.NickName = userid.text;
        userName.text = PhotonNetwork.NickName;
        Debug.Log(userid.text);

        if(userid != null)
        {
            Login_Pop.SetActive(false);
           
            Lobby_Camer.SetActive(false);
            Game_UI.SetActive(true);
            StartGame();
        }
        
        
    }
}