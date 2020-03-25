using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{

    public GameObject gameManager;
    // Start is called before the first frame update
    void Awake()
    {
        // 인스턴스 초기화
        if(GameManager.instance == null){
            Instantiate(gameManager);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
