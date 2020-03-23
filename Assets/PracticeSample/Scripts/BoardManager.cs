using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public class Count{
        public int minimum;
        public int maximum;

        public Count (int min, int max){
            minimum = min;
            maximum = max;
        }
    }

    // 게임보드 행, 열 생성
    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count (5,9);       // 벽 랜덤 생성 범위 지정
    public Count foodCount = new Count (1,5);       // 음식 숫자
    public GameObject exit;         // 출구
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;      // hierarchy를 깨끗이 하기 위해 사용
    private List <Vector3> gridPositions = new List<Vector3>();

    void InitializeList(){      
        gridPositions.Clear();  // 리스트 내부의 clear 함수를 써서 모든 리스트 된 gridposition 초기화

        // 여기선 마지막 가장자리 floor를 남기려고 - 1을 준거임
        for (int x=1; x < columns - 1; x++){                // 6 x 6 타일을 오브젝트로 하나씩 채워넣기 위해 벡터 넣음
            for (int y = 1; y < rows - 1; y++){
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    // 바깥벽과 게임 보드의 바닥을 짓기 위해 사용
    void BoardSetup(){  
        boardHolder = new GameObject ("Board").transform;       // ??
        for (int x = -1; x < columns + 1; x++){                 // 바깥 벽 타일 설정
            for (int y = -1; y < rows + 1; y++){
                // 여기 조금 어려움..
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows){
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;     // 미친..
            }
        }
    }

    // 적, 음식 등 배치를 위한 랜덤값 설정
    Vector3 RandomPosition(){
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    // 랜덤 배치
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i< objectCount; i++){
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    // scene 설정 (start, update 다 지움)
    public void SetupScene(int level){
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int) Mathf.Log(level, 2f);        // enemy 숫자 증가
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns-1, rows-1, 0F), Quaternion.identity); // 출구 설정
    }
}
