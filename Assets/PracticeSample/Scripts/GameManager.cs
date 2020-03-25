﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤
    public static GameManager instance = null;

    public BoardManager boardScript;
    private int level = 3;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
