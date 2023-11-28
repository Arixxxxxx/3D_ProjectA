using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Inst;

    [SerializeField] GameObject[] DmgFont_Obj;
    [SerializeField] GameObject[] BoomBullet3EA;

    Queue<GameObject>[] pool;

    [SerializeField] int FontOBJ_EA;
    [SerializeField] int BoomOBJ_EA;


    Transform[] PoolParent;
    
   
    
    
    private void Awake()
    {
        if(Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }


        Init();
    }
    void Start()
    {
        
    }
     

    private void Init()
    {
        int ObjCount = DmgFont_Obj.Length;
        PoolParent = new Transform[ObjCount];
        PoolParent[0] = transform.Find("ObjPool/DmgFont").transform;
        
        pool = new Queue<GameObject>[ObjCount];

        for(int i = 0; i < ObjCount; i++)
        {
            pool[i] = new Queue<GameObject>();
        }

        for(int i = 0;i < FontOBJ_EA; i++)
        {
            GameObject obj = Instantiate(DmgFont_Obj[0], transform.position, Quaternion.identity, PoolParent[0]);
            pool[0].Enqueue(obj);
            obj.SetActive(false);
        }

    }


    /// <summary>
    /// 오브젝트 풀링 [ 0 대미지폰트 / ]
    /// </summary>
    /// <param name="Value"></param>
    public GameObject F_GetObj(int Value)
    {
        GameObject obj;
        switch (Value)
        {
            case 0:
          
                if (pool[0].Count <= 0)
                {
                    GameObject obj1 = Instantiate(DmgFont_Obj[0], transform.position, Quaternion.identity, PoolParent[0]);
                    pool[0].Enqueue(obj1);
                    obj1.SetActive(false);

                }

                obj = pool[0].Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
        }

        return default;
    }

    /// <summary>
    /// Obj 반환
    /// </summary>
    /// <param name="obj">게임오브젝트</param>
    /// <param name="value">0=폰트 / </param>
    public void F_ReturnObj(GameObject obj, int value)
    {
        switch(value)
        {
            case 0:
                obj.SetActive(false);
                obj.transform.SetParent(PoolParent[0]);
                obj.transform.position = Vector3.zero;
                pool[0].Enqueue(obj);
                break;
        }
    }
}
