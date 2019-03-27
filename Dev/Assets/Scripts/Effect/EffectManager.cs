using Scene;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace effect
{
    [SLua.CustomSingletonLuaClass]
    public class EffectManager : Singleton<EffectManager>
    {
        private List<EffectObject> _listEffect = new List<EffectObject>();

        #region MOVOBEHAVIOUR
        // 初始化
        /*void Start()
        {

        }*/

        // Update 每帧调用一次
        void Update()
        {
            for (int i = _listEffect.Count - 1; i >= 0; i--)
            {
                if (_listEffect[i].isAutoDestroy() && _listEffect[i].isStopped())
                {
                    EffectObject effectObj = _listEffect[i];
                    _listEffect.RemoveAt(i);
                    effectObj.destroy();
                }
            }
        }
        #endregion

        public void initialize()
        {

        }


        //获取光效路径
        public static string getEffectFilePath(string strEffectName)
        {
            return "Resources/Effects/" + strEffectName;
        }

        //添加光效
        public EffectObject addEffect(Transform target, string strEffectName, bool bLoop = false, int offset = 20, float fScale = 1f, float fSpeed = 1f,bool isAutoDestroy=true)
        {
            if (target == null)
            {
                return null;
            }

            EffectObject effect = new EffectObject(strEffectName, isAutoDestroy);
            if (!effect.Equals(null))
            {
                effect.Offset = offset;
                effect.setParent(target);
                effect.setLayer(target.gameObject.layer);
                effect.Loop = bLoop;
                effect.play();
                effect.SetScale(fScale);
                effect.SetSpeed(fSpeed);
            }

            return effect;
        }


        public EffectObject addEffectByPos(Vector3 target, string strEffectName, bool bLoop = false)
        {

            EffectObject effect = new EffectObject(strEffectName);
            if (!effect.Equals(null))
            {
                effect.setParent(GameSceneManager.getInstance().CurSceneObject.transform);
                effect.setPosition(target);
                effect.setLayer(LayerMask.NameToLayer("UI"));
                effect.Loop = bLoop;
                effect.play();
            }
            return effect;
        }

        public EffectObject CreateEffect(Transform target, string strEffectName, Vector3 pos, bool bLoop = false)
        {
            var effect = new EffectObject(strEffectName);
            effect.setParent(target);
            effect.setLayer(target.gameObject.layer);
            effect.EffectGameObj.transform.localPosition = pos;
            effect.Loop = bLoop;
            effect.play();
            return effect;
        }

        // 创建光效
        public EffectObject CreateEffect(string strEffectName, bool bAutoDestroy, Vector3 v3Position, Quaternion quat, bool bLoop = false)
        {
            var effect = new EffectObject(strEffectName, bAutoDestroy);
            effect.EffectGameObj.transform.position = v3Position;
            effect.EffectGameObj.transform.rotation = quat;
            effect.Loop = bLoop;
            effect.play();
            return effect;
        }


        //销毁光效
        public void DestroyEffect(EffectObject effect)
        {
            if (effect == null)
            {
                return;
            }

            effect.destroy();

            if (_listEffect.Contains(effect))
            {
                _listEffect.Remove(effect);
            }
        }

        //添加
        public void addList(EffectObject effect)
        {
            if (effect != null)
            {
                _listEffect.Add(effect);
            }
        }

        //移除特效
        public void removeFromList(EffectObject effect)
        {
            if (effect != null && _listEffect.Contains(effect))
            {
                _listEffect.Remove(effect);
            }
        }
    }

}

