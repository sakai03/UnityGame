using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OnlyForwardSearch : MonoBehaviour
{
    [SerializeField]
    private OnlyForwardSearch onlyForwardSearchEnemy;
    [SerializeField]
    private SphereCollider searchArea;
    [SerializeField]
    private float searchAngle = 130f;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            //主人公の方向
            Vector3 playerDirection = other.transform.position - transform.position;
            //敵の前方からの主人公の方向()
            float angle = Vector3.Angle(transform.forward, playerDirection);
            //サーチする角度内だったら発見
            if(angle <= searchAngle)
            {
                Debug.Log("主人公発見:" + angle);
                //onlyForwardSearchEnemy.
            }
        }
    }
}
