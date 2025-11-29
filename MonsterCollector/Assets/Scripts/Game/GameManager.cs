using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake() { instance = this; }

    [Header("Game Settings")]
    [SerializeField]
    private ColorVersion colorVersion = ColorVersion.None;
    public ColorVersion GetColorVersion() { return colorVersion; }

    [Header("Player Settings")]
    [SerializeField]
    private float playerMoveSpeed;
    public float GetPlayerMoveSpeed() { return playerMoveSpeed; }
    
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1200, 1080, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            Camera.main.transform.position = new Vector3(playerObject.transform.position.x, 1, playerObject.transform.position.z);
        }
    }
}
