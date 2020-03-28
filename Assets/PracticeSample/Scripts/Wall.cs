using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite dmgSprite;
    public int hp = 4;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    public void DamageWall (int loss){
        // 플레이어가 성공적으로 벽을 공격했을 떄 시각적인 변화를 줌
        spriteRenderer.sprite = dmgSprite;
        // 벽 파괴
        hp -= loss;
        if(hp <= 0) gameObject.SetActive(false);
    }
}
