using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPGSYSTEM
{
   public class Skill<T> : MonoBehaviour
    {
        T data;
        int skill_LV=1;
        
        public virtual void Init(T data)
        {
            this.data = data;
            skill_LV = 1;
        }

        public virtual void OnSkill() { }
        public virtual void Skilling() { }
        public virtual void OffSkill() { }

    }
}
