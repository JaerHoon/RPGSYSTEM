using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPGSYSTEM;
using System;

namespace RPGSYSTEM.UI
{
    //UI ��Ģ
    // 1. UIŬ������ �����Ǵ� ������Ʈ���� ������� ����, ��ư.. �̷��� �ݵ�� �θ� ����� �Ѵ�.(1���� �־)
    // 2. �׸��� �� �θ𿡰Ը� �±׸� ������. �̷��� �ϴ� ������ �� ��ҵ��� ��� �����ϰ� �ͱ� �����̴�.
    

    public abstract class UI : MonoBehaviour
    {

        protected Dictionary<Enums.UIType, List<List<object>>> UIList = new Dictionary<Enums.UIType, List<List<object>>>();

        //�����ɷ� ĳ���� �ѵڿ� ���� ���� ������ �ְ� init�� �����ؼ� ������Ʈ�� �Ҵ����ش�.
        protected virtual void setting()
        {
            //int eVaues = Enum.GetValues(typeof(Enums.UIType)).Length;
          
            Slots Slot1 = CastingUI<Slots>(typeof(Slots), 0, 1)[0];
        }

        // ������� ĳ���� �ؼ� ����Ʈ�� ���ϴ� �Լ�
        protected virtual List<T> CastingUI<T>(Type type,int typeNum ,int oder)
        {
            Enums.UIType uitype = (Enums.UIType)Enum.GetValues(typeof(Enums.UIType)).GetValue(typeNum);
            List<T> uiobjlist = new List<T>();
            for(int i=0;i< UIList[uitype][oder].Count; i++)
            {
               T asd = (T)UIList[uitype][oder][i];
                uiobjlist.Add(asd);
            }

            return uiobjlist;

        }

        protected virtual void InIt() 
        {
            List<string> uitypes = new List<string>();
            uitypes = Utility.EnumToStringList<Enums.UIType>(); 
            // Tag �˻��� ���� UIType�̳��� ��Ʈ�� Ÿ�� ����Ʈ������ �ִ´�.

            
            //UI Ÿ���� ���� ��ŭ �ݺ��Ѵ�.
            for (int i = 0; i < uitypes.Count; i++)
            {
                //�ڽ� �߿��� i��° �±׸� ������ �ִ� ������Ʈ�� ����Ʈ�� �����Ѵ�.
                List<GameObject> gameObjects = Utility.FindChildrenWithTag(this.gameObject, uitypes[i]);


                // �±׸� ������ �ִ� ù��° ������Ʈ�� �ڽ� ������Ʈ ��ŭ �ݺ��Ѵ�.
                for(int a=0; a < gameObjects[i].transform.childCount; a++)
                {
                    //UIŸ�� �̳��� i��° ��Ҹ� �����´�.
                    Enums.UIType uitype = (Enums.UIType)Enum.GetValues(typeof(Enums.UIType)).GetValue(i);

                    //��ųʸ��� ���� ������Ʈ ����Ʈ ����Ʈ�� �����Ѵ�.
                    List<List<object>> uiclass = new List<List<object>>();
                    
                    //�̹��� ����� Ŭ���� Ÿ���� �����´�. UIŸ�԰� ��ġ
                    Type ty = Types.type[i];

                    //������ ���� ������Ʈ ����Ʈ ����Ʈ�� GetClass���� ��ȯ��
                    //���� ���� Ŭ���� ��ü�� ���� ������Ʈ����Ʈ�� �����Ѵ�.
                    uiclass.Add(GetClass(gameObjects[i], ty));

                    //�׸��� ���� ����Ʈ�� ��ųʸ��� UIŸ���� Ű���� �Բ� �ִ´�.
                    UIList.Add(uitype, uiclass);
                }
            }

            //������� �Ѵٸ� �� ���ӿ�����Ʈ�� �ڽĿ��� ���� ��� UIŸ���� Ŭ������ ��ųʸ��� ����.

            setting(); // ���� ��ųʸ��� ��ü�ؼ� ���� �̿��� �� �ִ� ������ ����. 
        }

        // �±� Ÿ�� ������Ʈ�� �ڽ� ����ŭ ��ü�� �����ؼ� ������Ʈ ����Ʈ�� �־� ��ȯ�Ѵ�.
        protected virtual List<object> GetClass(GameObject obj, Type type)
        {
            List<object> list = new List<object>();

            for (int i=0;i< obj.transform.childCount; i++)
            {
                object instance = Activator.CreateInstance(type);
                list.Add(instance);
            }


            return list;
        }

       

    }


    public class Slots 
    {
        Image icon;
        Image frame;
        Image backGround;

       
    }

    public class Buttons
    {

    }
   
}