using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이전에 작성했던 MovingObject 상속
public class PlayerController : MovingObject
{
    public int wallDamage = 1;      // 플레이어가 벽을 부술때 적용되는 데미지
    public int pointsPerFood = 10;  // 음식 점수
    public int pointsPerSoda = 20;  // 소다 점수
    public float restartLevelDelay = 1f;

    private Animator animator;      // 애니메이터 컴포넌트의 레퍼런스를 가져오기 위해 사용
    private int food;

    // Start is called before the first frame update
    protected override void Start()     // player의 start를 구현하기 위해 선언해줌..?
    {
        animator = GetComponent<Animator>();
        // 해당 레밸 동안 음식점수를 관리하기 위해 저장
        food = GameManager.instance.playerFoodPoints;       // game manager에 있는 food point를 가져옴

        base.Start();   // moving object의 start를 부른다는 거 인듯..?
    }

    private void OnDisable() {      // 플레이어가 disable 될때 호출됨
        // 레밸이 변환될 때 게임매니저에 food 값을 저장하는데 사용함
        GameManager.instance.playerFoodPoints = food;
    }

    // attempt move 함수
    protected override void AttemptMove <T> (int xDir, int yDir){        // 여기서 T는 움직이는 오브젝트가 마주칠 대상의 컴포넌트의 타입을 가리키려고 사용함
        food -= 1;  // 음식점수 하나 감소
        base.AttemptMove <T> (xDir, yDir);      // 부모 클래스의 attempt move 호출
        RaycastHit2D hit;
        CheckIfGameOver();      // 플레이어가 움직이면서 음식 점수를 잃기 때문에 게임이 끝났는지 확인해야함
        GameManager.instance.playersTurn = false;       // 플레이어 턴이 끝났음을 알림
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어턴이 아니라면 코드들을 실행하지 않도록 해줌
        if (!GameManager.instance.playersTurn) return;

        // 이동 처리
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");

        if (horizontal != 0) vertical = 0;      // 대각선 이동 방지

        if (horizontal != 0 || vertical != 0){      //  플레이어 이동 감지
            AttemptMove<Wall> (horizontal, vertical);   
        }
    }

    // 모든 오브젝트와 상호 작용을 하기 위해 ontriggerenter2d 사용
    private void OnTriggerEnter2D(Collider2D other) {
        // 출구에 도착한다면
        if (other.tag == "Exit"){
            Invoke("Restart", restartLevelDelay);       // 1초 후 재시작
            enabled = false;
        }else if (other.tag == "Food"){             // 음식을 먹었다면
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        }else if (other.tag == "Soda"){             // 소다를 먹었다면
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }

    // 게임 오버
    private void CheckIfGameOver(){
        // 음식 점수가 0이라면
        if(food <= 0){
            GameManager.instance.GameOver();
        }
    }

    // 잃는 점수 계산
    public void LoseFood(int loss){
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    // 레밸 리로드를 위한 함수 
    private void Restart(){     // 출구에 도착했을 경우 
        Application.LoadLevel(Application.loadedLevel);
    }

    // 플레이어가 이동하려는 공간에 벽이 있고 이에 막히는 경우의 행동
    protected override void OnCantMove <T> (T component){
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);     // 플레이어가 얼마나 벽에 데미지를 줄지 알리기 위해 wallDamage를 입력해서 넣어줌
        animator.SetTrigger("playerChop");  // 애니메이터 컴포넌트의 chop 트리거 호출
    }
}
