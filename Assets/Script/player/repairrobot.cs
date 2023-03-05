using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class repairrobot : MonoBehaviour
{
    [SerializeField] float RotateSpeed = 5f;
    [SerializeField] float Radius = 0.5f;
    [SerializeField] int regen;
    GameObject player;
    private float _angle;
    GameObject gameco;
    void OnEnable()
    {
        //Radius = transform.parent.GetComponent<CircleCollider2D>().radius;
        gameco = GameObject.Find("GC");
        player = transform.parent.gameObject;
        //StopCoroutine(playerc.player_move_coroutine);
        transform.parent.GetComponent<playerc>().stop_move();
        player.GetComponent<playerc>().repair_robot_regen(regen);
        StartCoroutine(robot_move());
    }
    private void OnDisable()
    {
        if(player.activeInHierarchy)
        player.GetComponent<playerc>().base_regen();
        StopCoroutine(robot_move());
    }
    IEnumerator robot_move()
    {
        while(true)
        {
            _angle += RotateSpeed * Time.deltaTime;

            var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
            transform.position = (Vector2)player.transform.position + offset;
            transform.up = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            yield return 0;
        }
    }
}