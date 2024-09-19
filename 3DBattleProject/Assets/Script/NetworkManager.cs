using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // ���� �Է�
    private readonly string version = "1.0f";
    // ����� ���̵� �Է�
    [SerializeField] private InputField userid;
    [SerializeField] private GameObject Login_Pop;
    [SerializeField] private GameObject Game_UI;
    [SerializeField] private GameObject Lobby_Camer;
    [SerializeField] private TextMeshProUGUI userName;
    private void StartGame()
    {
        DontDestroyOnLoad(this);
        // ���� ���� �����鿡�� �ڵ����� ���� �ε�
        PhotonNetwork.AutomaticallySyncScene = true;
        // ���� ������ �������� ���� ���
        PhotonNetwork.GameVersion = version;
        // ���� ������ ��� Ƚ�� ����. �ʴ� 30ȸ
        Debug.Log(PhotonNetwork.SendRate);
        // ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }
    // ���� ������ ���� �� ȣ��Ǵ� �ѹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby(); //�κ� ����
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom(); //���� ��ġ����ŷ ��� ����
    }

    // ������ �� ������ �������� ��� ȣ��Ǵ� �ݹ� �Լ�

    public override void OnJoinRandomFailed(short returnCord, string message) 
    {
        Debug.Log($"JoinRandom Filed {returnCord} : {message}"); // �������� �ʾ����� ����

        // ���� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4; // �ִ� ������ �� : 4��
        ro.IsOpen = true; // ���� ���� ����
        ro.IsVisible = true;  // �κ񿡼� �� ��Ͽ� ���� ��ų�� ���� (����,�����)

        // �� ����
        PhotonNetwork.CreateRoom("My Room", ro);
    }

    // �� ������ �Ϸ�� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    // �뿡 ������ �� ȣ��Ǵ� �ѹ� �Լ�
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        // �뿡 ������ ����� ���� Ȯ��
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName}.{player.Value.ActorNumber}");
            // $ => String.Format()
        }
        int idx_Num = PhotonNetwork.LocalPlayer.ActorNumber;
        // ĳ���� ���� ������ �迭�� ����
        Transform[] points = GameObject.Find("Map").GetComponentsInChildren<Transform>();

        if (idx_Num == 1)
        {
           
            int idx = Random.Range(1, 2);
            // ĳ���͸� ����

            PhotonNetwork.Instantiate("PlayerCharacter", points[idx].position, points[idx].rotation, 0);
        }
        else if (idx_Num <= 2)
        {
           
            int idx = Random.Range(3, points.Length);
            // ĳ���͸� ����

            PhotonNetwork.Instantiate("PlayerCharacter 1", points[idx].position, points[idx].rotation, 0);
        }
    }
    public void Checkname()
    {
        // ���� ���̵� �Ҵ�
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