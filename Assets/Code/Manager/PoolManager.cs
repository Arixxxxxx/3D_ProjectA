using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Inst;

    int ObjCount = 2;
    [SerializeField] GameObject DmgFont_Obj;
    [SerializeField] GameObject[] BoomBullet3EA;
    [SerializeField] GameObject[] Particle;

    [SerializeField] Queue<GameObject>[] pool;
    [SerializeField] Transform[] PoolParent;

    [SerializeField] int FontOBJ_EA;
    [SerializeField] int BoomOBJ_EA;
    [SerializeField] int TelePort_PS_EA;

 
  
    
   
    
    
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
       
        PoolParent = new Transform[2];
        pool = new Queue<GameObject>[2];

        PoolParent[0] = transform.Find("ObjPool/DmgFont").transform;
        PoolParent[1] = transform.Find("ObjPool/Teleport").transform;

        for(int i = 0; i < pool.Length; i++)
        {
            pool[i] = new Queue<GameObject>();
        }

        for(int i = 0;i < FontOBJ_EA; i++)
        {
            GameObject obj = Instantiate(DmgFont_Obj, transform.position, Quaternion.identity, PoolParent[0]);
            pool[0].Enqueue(obj);
            obj.SetActive(false);
        }

        //텔레포트PS
        for(int i = 0; i< TelePort_PS_EA; i++)
        {
            GameObject obj = Instantiate(Particle[0], transform.position, Quaternion.Euler(new Vector3(-90,0,0)), PoolParent[1]);
            pool[1].Enqueue(obj);
            obj.SetActive(false);
        }

    }


    /// <summary>
    /// 오브젝트 풀링 [ 0 = 대미지폰트 , 1 = 텔레포트 파티클, ]
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
                    GameObject obj1 = Instantiate(DmgFont_Obj, transform.position, Quaternion.identity, PoolParent[0]);
                    pool[0].Enqueue(obj1);
                    obj1.SetActive(false);

                }

                obj = pool[0].Dequeue();
                obj.gameObject.SetActive(true);
                return obj;

            case 1:
                if (pool[1].Count <= 0)
                {
                    GameObject obj2 = Instantiate(Particle[0], transform.position, Quaternion.identity, PoolParent[1]);
                    pool[1].Enqueue(obj2);
                    obj2.SetActive(false);
                }
                obj = pool[1].Dequeue();
                

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

            case 1:
                obj.SetActive(false);
                obj.transform.SetParent(PoolParent[1]);
                obj.transform.position = Vector3.zero;
                pool[1].Enqueue(obj);
                break;
        }

        
    }
}
