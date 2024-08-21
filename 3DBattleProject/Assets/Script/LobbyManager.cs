using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public GameObject Login_Pop;
    public InputField Uname;
    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Checkname()
    {
       string username = Uname.text;

       Debug.Log(username);

       Login_Pop.SetActive(false);
    }
}
