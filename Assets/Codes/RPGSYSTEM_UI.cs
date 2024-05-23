using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPGSYSTEM;
using System;

namespace RPGSYSTEM.UI
{
    //UI 규칙
    // 1. UI클래스에 대응되는 오브젝트들은 예를들어 슬롯, 버튼.. 이런건 반드시 부모에 묶어야 한다.(1개만 있어도)
    // 2. 그리고 그 부모에게만 태그를 입힌다. 이렇게 하는 이유는 그 요소들을 묶어서 관리하고 싶기 때문이다.
    

    public abstract class UI : MonoBehaviour
    {

        protected Dictionary<Enums.UIType, List<List<object>>> UIList = new Dictionary<Enums.UIType, List<List<object>>>();

        //원래걸로 캐스팅 한뒤에 내가 만든 변수에 넣고 init을 실행해서 컴포넌트를 할당해준다.
        protected virtual void setting()
        {
            //int eVaues = Enum.GetValues(typeof(Enums.UIType)).Length;
          
            Slots Slot1 = CastingUI<Slots>(typeof(Slots), 0, 1)[0];
        }

        // 원래대로 캐스팅 해서 리스트로 반하는 함수
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
            // Tag 검색을 위해 UIType이넘을 스트링 타입 리스트로으로 넣는다.

            
            //UI 타입의 갯수 만큼 반복한다.
            for (int i = 0; i < uitypes.Count; i++)
            {
                //자식 중에서 i번째 태그를 가지고 있는 오브젝트를 리스트에 저장한다.
                List<GameObject> gameObjects = Utility.FindChildrenWithTag(this.gameObject, uitypes[i]);


                // 태그를 가지고 있는 첫번째 오브젝트의 자식 오브젝트 만큼 반복한다.
                for(int a=0; a < gameObjects[i].transform.childCount; a++)
                {
                    //UI타입 이넘의 i번째 요소를 가져온다.
                    Enums.UIType uitype = (Enums.UIType)Enum.GetValues(typeof(Enums.UIType)).GetValue(i);

                    //딕셔너리에 담을 오브젝트 리스트 리스트를 생성한다.
                    List<List<object>> uiclass = new List<List<object>>();
                    
                    //이번에 사용할 클래스 타입을 가져온다. UI타입과 일치
                    Type ty = Types.type[i];

                    //위에서 만든 오브젝트 리스트 리스트에 GetClass에서 반환한
                    //새로 만든 클래스 객체를 담은 오브젝트리스트를 삽입한다.
                    uiclass.Add(GetClass(gameObjects[i], ty));

                    //그리고 위의 리스트를 딕셔너리에 UI타입의 키값과 함께 넣는다.
                    UIList.Add(uitype, uiclass);
                }
            }

            //여기까지 한다면 이 게임오브젝트의 자식에게 속한 모든 UI타입의 클래스가 딕셔너리에 담겼다.

            setting(); // 이제 딕셔너리를 해체해서 내가 이용할 수 있는 변수에 담자. 
        }

        // 태그 타입 오브젝트의 자식 수만큼 객체를 생성해서 오브젝트 리스트에 넣어 반환한다.
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