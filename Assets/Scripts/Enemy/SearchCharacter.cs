using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCharacter : MonoBehaviour
{
    private MoveEnemy moveEnemy;
    void Start()
    {
        moveEnemy = GetComponentInParent<MoveEnemy>();
        
    }

    void OnTriggerStay(Collider other)
    {
        //プレイヤータグだったら発見
        if(other.tag == "Player")
        {
            MoveEnemy.EnemyState state = moveEnemy.GetState();
            if (state != MoveEnemy.EnemyState.Chase) {
                Debug.Log("プレイヤー発見");
                moveEnemy.SetState(MoveEnemy.EnemyState.Chase, other.transform);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("見失った");
            moveEnemy.SetState(MoveEnemy.EnemyState.Idle);
        }
    }
}
