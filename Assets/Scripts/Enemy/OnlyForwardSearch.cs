using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OnlyForwardSearch : MonoBehaviour
{
    [SerializeField]
    private OnlyForwardSearch onlyForwardSearch;
    private MoveEnemy moveEnemy;
    [SerializeField]
    private SphereCollider searchArea;
    [SerializeField]
    private float searchAngle = 130f;

    private void Start()
    {
        moveEnemy = GetComponentInParent<MoveEnemy>();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            MoveEnemy.EnemyState state = moveEnemy.GetState();
            //主人公の方向
            Vector3 playerDirection = other.transform.position - transform.position;
            //敵の前方からの主人公の方向()
            float angle = Vector3.Angle(transform.forward, playerDirection);
            //サーチする角度内だったら発見
            if(angle <= searchAngle)
            {
                if(state != MoveEnemy.EnemyState.Chase)
                {
                    Debug.Log("主人公発見:" + angle);
                    moveEnemy.SetState(MoveEnemy.EnemyState.Chase, other.transform);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            moveEnemy.SetState(MoveEnemy.EnemyState.Idle);
        }
    }
#if UNITY_EDITOR
    //サーチする角度を確認
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
    }
#endif
}