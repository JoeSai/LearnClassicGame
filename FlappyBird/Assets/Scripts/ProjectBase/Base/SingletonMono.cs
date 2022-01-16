using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�̳�MonoBehaviour�ĵ���ģʽ����
//��Ҫ��֤������ֻ����һ���ű�

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        //�̳���Mono�Ļ��� ����ֱ��new
        //ֻ��ͨ���϶��������� ���� AddComponent�ӽű�
        return instance;
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
}
