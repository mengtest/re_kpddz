using UnityEngine;
using System.Collections;
using EventManager;
using player;

namespace effect
{
    public class EffectLineBehaviour : EffectDefaultBehaviour
    {

        // 初始化
        public override void Start()
        {

        }

        // Update 每帧调用一次
        //public override void Update()
        //{

        //}

        public override void actionStart()
        {
          
        }

        public virtual void onReachDestination(System.Object args)
        {
            if (args != null)
            {
//                 EventMultiArgs argsSSS = args as EventMultiArgs;
//                 EffectObject effect = argsSSS.GetArg<EffectObject>("refData", null);
//                 PlayerObjectBase target = argsSSS.GetArg<PlayerObjectBase>("target", null);
// 
//                 EffectManager.getInstance().addEffect(target._modelRef.ModelRootObj.transform, "diaochan/ducishouji");
//                 target._modelRef.playAnimationsAndReset(new int[] { 501 });
// 
//                 effect.destroy();
            }
        }
    }
}

