using System;
using System.Collections;

namespace MyExtensionMethod
{
    /// <summary>
    /// 通知类
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// 是否具备执行条件
        /// </summary>
        public bool IsReady { get { return _isReady(); } }
        /// <summary>
        /// 无参数委托
        /// </summary>
        private Action _noParamAction = null;
        /// <summary>
        /// 带参数委托
        /// </summary>
        private Action<Hashtable> _paramAction = null;
        /// <summary>
        /// 条件判断函数
        /// </summary>
        private readonly Func<bool> _isReady;

        /// <summary>
        /// 通知类
        /// </summary>
        /// <param name="func">条件判断函数</param>
        public Notification(Func<bool> func)
        {
            _isReady = func;
        }
        
        /// <summary>
        /// 执行全都委托
        /// </summary>
        /// <param name="args">参数</param>
        public void Dispatch(Hashtable args = null)
        {
            if (!IsReady) return;
            if (_noParamAction != null) {
                _noParamAction();
            }

            if (_paramAction != null && args != null) {
                _paramAction(args);
            }
        }

        /// <summary>
        /// 添加并根据 IsReady 判断是否执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="args"></param>
        public void AddAndDispatch(Action<Hashtable> action, Hashtable args)
        {
            _paramAction += action;
            if (IsReady && args != null) {
                action(args);
            }
        }

        /// <summary>
        /// 添加并根据 IsReady 判断是否执行
        /// </summary>
        /// <param name="action"></param>
        public void AddAndDispatch(Action action)
        {
            if (action == null)
                return;
            _noParamAction += action;
            if (IsReady) {
                action();
            }
        }
       
        /// <summary>
        /// 从委托列表中移除 action
        /// </summary>
        /// <param name="action"></param>
        public void Add(Action action)
        {
            if (action == null)
                return;
            _noParamAction += action;
        }

        /// <summary>
        /// 从委托列表中移除 action
        /// </summary>
        /// <param name="action"></param>
        public void Add(Action<Hashtable> action)
        {
            if (action == null)
                return;
            _paramAction += action;
        }   
        
        /// <summary>
        /// 从委托列表中移除 action
        /// </summary>
        /// <param name="action"></param>
        public void Remove(Action action)
        {
            if (action == null)
                return;
            _noParamAction -= action;
        }

        /// <summary>
        /// 从委托列表中移除 action
        /// </summary>
        /// <param name="action"></param>
        public void Remove(Action<Hashtable> action)
        {
            if (action == null)
                return;
            _paramAction -= action;
        }

        /// <summary>
        /// 清空委托
        /// </summary>
        public void ClearDelegate()
        {
            _noParamAction = null;
            _paramAction = null;
        }
    }
}

