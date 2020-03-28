using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public abstract
public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;       // 이동속도
    public LayerMask blockingLayer;     // 충돌이 일어났는지 확인

    // 이동을 하기 위해 boxcollider, rigid, speed 설정
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;


    // Start is called before the first frame update
    // 재정의를 위해 pretected virtual 사용
    // start를 다르게 설정해야할때 유용함
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f/ moveTime;
    }

    // move bool 메서드 생성
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit){
        Vector2 start = transform.position; // z 축 값이 날아가면서 반환됨
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;        // raycast를 사용할 때 본인과 충돌하지 않기 위해 해제함
        hit = Physics2D.Linecast (start, end, blockingLayer) ;  // ????
        boxCollider.enabled = true;

        if(hit.transform == null){      // 뭔가 부딪혔는지 null 체크 확인
            StartCoroutine(SmoothMovement (end));
            return true;        // null인 것은 아무것도 없으므로 이동할 수 있다는 뜻이므로 이동 시킴
        }

        return false;       // 그 외에는 앞에 뭔가 있으므로 false 
    }

    // 코루틴 이동 설정
    protected IEnumerator SmoothMovement(Vector3 end){
        // end와 현재 위치 차이 벡터에 magintude로 구함
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        // ...?? 이거 아마 자동 이동 처리하는거 같은데
        while (sqrRemainingDistance > float.Epsilon){
            // movetowards를 사용해 원하는 지점으로 이동시시킴
            // rb2d.position - 현재 위치, end - 목적지, inverse... - 이동속도
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected abstract void OnCantMove<T> (T component)
        where T : Component;

    protected virtual void AttemptMove<T> (int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null) return;
        
        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)  OnCantMove(hitComponent);

    }
}
