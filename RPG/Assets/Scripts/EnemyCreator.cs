using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [Tooltip("生成范围")]
    public Vector3 range;
    [Tooltip("怪物生成数量")]
    public int monsterNumber;
    [Tooltip("怪物数量低于多少会开始生成")]
    public int monsterMin;

    public List<GameObject> monsterObjs;
    private BoxCollider boxCollider;

    // 怪物当前数量
    private int presentMonsterNumber;
    void Start()
    {
        StartCoroutine(CreatorMonster());
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = range * 2.5f;
    }
    void Update()
    {
        
    }
    IEnumerator CreatorMonster()
    {
        bool isCreator = true;
        while(true)
        {
            if(isCreator == false)
            {
                // 如果当前怪物数量低于最小值就开始生成
                if(presentMonsterNumber < monsterMin)
                {
                    isCreator = true;
                }
            }
            else
            {
                Ray ray = new Ray(new Vector3(Random.Range(transform.position.x - range.x, transform.position.x + range.x),
                    transform.position.y,
                    Random.Range(transform.position.z - range.z, transform.position.z + range.z)), -transform.up);
                if(Physics.Raycast(ray, out RaycastHit hit, range.y, LayerMask.GetMask("Map")))
                {
                    GameObject temp = Instantiate(monsterObjs[Random.Range(0, monsterObjs.Count)], hit.point, Quaternion.identity);
                    Enemy enemy = temp.transform.GetChild(0).GetComponent<Enemy>();
                    enemy.range = range;
                    enemy.monsterCreatorCenter = transform.position;
                    enemy.OnDie += MonsterDie;
                    presentMonsterNumber++;
                }
                // 如果当前怪物数量大于最小值就停止生成
                if(presentMonsterNumber >= monsterMin)
                {
                    isCreator = false;
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }
    // 怪物死亡事件回调
    private void MonsterDie()
    {
        presentMonsterNumber--;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(100, 0, 0, 0.3f);
        Gizmos.DrawCube(transform.position, range * 2);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            AudioManage.single.audioState = AudioState.危险地带;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioManage.single.audioState = AudioState.安全地带;
        }
    }
}
