using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤
    public static GameManager instance = null;

    public BoardManager boardScript;
    private int level = 3;

    public int playerFoodPoints = 100;      // 음식점수
    [HideInInspector] public bool playersTurn = true;       // hide in inspector는 변수는 public이나 에디터에서 숨길 수 있음

    void Awake() {
        if (instance == null){
            instance = this;
        }else if(instance != this){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);      // 씬이 넘어가도 점수 계산을 해야하기 때문
        boardScript = GetComponent<BoardManager>();
        InitGame();    
    }

    // init game
    void InitGame(){
        boardScript.SetupScene(level);
    }

    // 게임오버 처리
    public void GameOver(){
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
