using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class MoveEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Walk,
        Idle,
        Chase
    };
    
    //private CharacterController enemyController;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    //目的地
    private Vector3 destination;
    //到着判定
    private bool arrived;
    //初期値記憶
    private Vector3 startPosition;
    //待機時間
    [SerializeField] private float waitTime = 2f;
    //経過時間
    private float elapsedTime;
    //敵の状態
    private EnemyState state;
    //追いかけるキャラクター
    private Transform playerTransform;
    //回転するスピード
    //private float rotateSpeed = 45f;

    void Start()
    {
        //コンポーネント取得
        //enemyController = GetComponent<CharacterController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //初期値設定
        arrived = false;
        startPosition = transform.position;
        elapsedTime = 0f;
        //メソッド呼び出し
        CreateRandomPosition();
        SetState(EnemyState.Walk);
    }

    void Update()
    {
        //見回り又はキャラクタを追いかける状態
        if (state == EnemyState.Walk || state == EnemyState.Chase)
        {
            //キャラクタを追いかける状態であればキャラクタの目的地を再設定
            if (state == EnemyState.Chase)
            {
                animator.SetFloat("Speed", 3.5f);
                destination = playerTransform.position;
                navMeshAgent.SetDestination(playerTransform.position);
                this.transform.LookAt(playerTransform);
            }
            //エージェントの潜在的な速度を設定
            animator.SetFloat("Speed", navMeshAgent.desiredVelocity.magnitude);
        }
        if (state == EnemyState.Walk)
        {
            //目的地に着いたかどうかの判定
            if (navMeshAgent.remainingDistance < 1.1f)
            {
                SetState(EnemyState.Idle);
                animator.SetFloat("Speed", 0.0f);
            }
        }
        else if (state == EnemyState.Idle)
        {
            elapsedTime += Time.deltaTime;
            //待機時間を超えたら次の目的地を再設定
            if (elapsedTime > waitTime)
            {
                SetState(EnemyState.Walk);
            }
        }
    }

    //ランダムな半径25m以内の数値を生成
    void CreateRandomPosition()
    {
        //半径8m以内のVector2オブジェクト生成,初期地点から設定
        Vector2 randDestination = Random.insideUnitCircle * 25f;
        destination = startPosition + new Vector3(randDestination.x, 0, randDestination.y);
    }

    public void SetState(EnemyState tempState,Transform targetObj=null)
    {
        if (tempState == EnemyState.Walk)
        {
            animator.SetFloat("Speed", 1.0f);
            arrived = false;
            elapsedTime = 0f;
            state = tempState;
            CreateRandomPosition();
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        } else if (tempState == EnemyState.Chase)
        {
            state = tempState;
            //待機状態から追いかける場合もあるのでOff
            arrived = false;
            //追いかける対象をセット
            playerTransform = targetObj.transform;
            navMeshAgent.SetDestination(playerTransform.position);
            navMeshAgent.isStopped = false;
        }
        else if(tempState == EnemyState.Idle)
        {
            elapsedTime = 0f;
            state = tempState;
            arrived = true;
            animator.SetFloat("Speed", 0.0f);
        }
    }
    //敵キャラクターの状態取得メソッド
    public EnemyState GetState()
    {
        return state;
    }
}
