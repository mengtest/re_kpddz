using UnityEngine;
using System.Collections;

namespace effect
{
    public class EffectDefaultBehaviour : MonoBehaviour
    {
        [HideInInspector]
        protected EffectObject _obj = null;

        //持续时间
        [HideInInspector]
        protected float _fTime = 0.0f;

        //目标
        protected Transform _targetTf = null;

        // 初始化
        public virtual void Start()
        {
            _obj = null;
            _fTime = 0;
            //_targetTf = null;
        }

        // Update 每帧调用一次
        public virtual void Update()
        {

        }

        public void OnBecameInvisible()
        {
            foreach (var ps in GetComponentsInChildren<ParticleSystem>())
            {
                ps.GetComponent<Renderer>().enabled = false;
            }
        }

        public void OnBecameVisible()
        {
            foreach (var ps in GetComponentsInChildren<ParticleSystem>())
            {
                ps.GetComponent<Renderer>().enabled = true;
            }
        }

        public void setTarget(Transform tfTarget)
        {
            if (tfTarget == null)
            {
                //Utils.LogSys.Log("Set target: NULL");
            }

            _targetTf = tfTarget;
        }

        public virtual void actionStart()
        {

        }
    }
}


