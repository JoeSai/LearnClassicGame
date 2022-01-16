using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ������
/// �ҵ������Լ�����µĿؼ�����
/// �ṩ��ʾ�������صķ���
/// </summary>
public class BasePanel : MonoBehaviour
{
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Slider>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<InputField>();
    }

    /// <summary>
    /// ��ʾ�Լ�
    /// </summary>
    public virtual void ShowMe()
    {
        //this.gameObject.SetActive
    }

    /// <summary>
    /// �����Լ�
    /// </summary>
    public virtual void HideMe()
    {
        //this.gameObject.SetActive
    }

    /// <summary>
    /// �õ���Ӧ���ֿؼ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controlName"></param>
    /// <returns></returns>
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            for(int i = 0; i< controlDic[controlName].Count; i++)
            {
                if(controlDic[controlName][i] is T)
                {
                    return controlDic[controlName][i] as T;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// �ҵ��Ӷ���Ķ�Ӧ�ؼ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();
      
        for (int i = 0; i < controls.Length; i++)
        {
            string objName = controls[i].gameObject.name;
                
            if (controlDic.ContainsKey(objName))
            {
                controlDic[objName].Add(controls[i]);
            }
            else
            {
                controlDic.Add(objName, new List<UIBehaviour>() { controls[i] });
            }

            if(controls[i] is Button)
            {
                // ����lambda ���ݲ���
                (controls[i] as Button).onClick.AddListener(()=>
                {
                    OnClick(objName);
                });
            }
            // ��ѡ����߶�ѡ��
            else if(controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChange(objName, value);
                });
            }
        }
    }


    protected virtual void OnClick(string btnName)
    {

    }

    protected virtual void OnValueChange(string toggleName ,bool value)
    {

    }
}
