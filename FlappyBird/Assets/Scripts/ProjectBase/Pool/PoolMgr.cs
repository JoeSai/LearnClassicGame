using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//�����е�һ������
public class PoolData{
    public GameObject fatherObj;
    public List<GameObject> poolList;

    public PoolData(GameObject obj , GameObject poolObj)
    {
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject> {};
        PushObj(obj);
    }

    public void PushObj(GameObject obj)
    {
        poolList.Add(obj);
        obj.transform.parent = fatherObj.transform;
        //������ʧ��
        obj.SetActive(false);
    }

    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        obj.transform.parent = null;
        obj.SetActive(true);
        return obj;
    }
}

//�����ģ��
//1.Directionary �� List
//2.GameObject �� Resource�������������е�API
public class PoolMgr : BaseManager<PoolMgr>
{
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    // �������ڵ�
    public GameObject poolObj;

    public void GetObj(string name , UnityAction<GameObject> callback)
    {
        if(poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            callback(poolDic[name].GetObj());
        }
        else
        {
            //ͨ���첽������Դ ����������ⲿ
            //obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            ResMgr.GetInstance().LoadAsync<GameObject>(name, (o) =>
            {
                //�Ѷ������ָĳɺͳ���һ��
                o.name = name;
                callback(o);
            });
      
        }
    }

    public void PushObj(string name , GameObject obj)
    {

        if (poolObj == null)
        {
            poolObj = new GameObject("Pool");
        }

        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        else
        {
            poolDic.Add(name, new PoolData(obj , poolObj));
        }
    }

    //how to use
    //get:
    // PoolMgr.GetInstance().GetObj("Test/Cube);"

    //push:
    //PoolMgr.GetInstance().push(this.gameObjcet.name, this.gameObject);

    //�����л�����ʱ���������
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
