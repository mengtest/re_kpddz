using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLua;

namespace FastAction
{
    [SLua.CustomLuaClass]
    public class FastMoveManager : MonoBehaviour
    {
        // 运行帧率
        private int _handleFrame = 60;
        // 每帧处理数量
        public int FrameHandleCount = 600;

        // 控制每帧执行数据
        private int _frameHandleIndex = 0;
        private bool _toNextFrame = false;
        private float _deltaTime = 0f;

        // 链表数据
        private FastMove _firstFastMove = null;
        private FastMove _currentFastMove = null;
        private FastMove _endFastMove = null;
        private FastMove _tempFastMove = null;
        public int _listCount = 0;

        public static int RenderIndex = 30;
        public void ResetRender()
        {
            FastMoveManager.RenderIndex = 30;
        }
        
        public int HandleFrame
        {
            get { return _handleFrame; }
            set { 
                _handleFrame = value;
                startTimer();
            }
        }

        void Start()
        {
            startTimer();
        }

        private void startTimer()
        {
            CancelInvoke();
            //float time = 1.0f / HandleFrame;
            _frameHandleIndex = 0;
            //InvokeRepeating("doing", time, time);
        }

        void OnDestroy()
        {
            if (_firstFastMove != null)
                _firstFastMove.Clean();
            _firstFastMove = null;
            _currentFastMove = null;
            _endFastMove = null;
            _tempFastMove = null;
            _listCount = 0;
            CancelInvoke();
        }

        void Update()
        {
            doing();
        }

        private void doing()
        {
            if (_currentFastMove == null || _listCount == 0) return;

            _deltaTime = Time.deltaTime;
            while (!_toNextFrame)
            {
                if (_currentFastMove.Doing(_deltaTime) == false)
                {
                    _toNextFrame = stopCurrent();
                }
                else
                {
                    _toNextFrame = beginNext();
                }
                _frameHandleIndex += 1;
                if (_frameHandleIndex == FrameHandleCount)
                {
                    _toNextFrame = true;
                }
            }
            _frameHandleIndex = 0;
            _toNextFrame = false;
        }

        public void Begin(FastMove fMove)
        {
            if (_firstFastMove == null)
            {
                _firstFastMove = fMove;
                _firstFastMove.Previous = null;
                _firstFastMove.Next = null;
                _currentFastMove = _firstFastMove;
                _endFastMove = _firstFastMove;
            }
            else
            {
                // 已经在链表中,不做处理
                if (_firstFastMove == fMove
                    || _endFastMove == fMove
                    || (fMove.Previous != null && fMove.Next != null))
                {
                    return;
                }
               
                _endFastMove.Next = fMove;
                fMove.Previous = _endFastMove;
                _endFastMove = fMove;
            }
            fMove.Prepare();
            _listCount += 1;
        }

        private bool beginNext()
        {
            if (_currentFastMove.Next != null)
            {
                _currentFastMove = _currentFastMove.Next;
                return false;
            }
            else
            {
                _currentFastMove = _firstFastMove;
                return true;
            }
        }
        private bool stopCurrent()
        {
            
            if (_currentFastMove != null && _currentFastMove.Previous == null)
            {
                // 前后都没有对象，列表最后一个执行完毕
                if (_currentFastMove.Next == null)
                {
                    if (_firstFastMove != null)
                        _firstFastMove.Clear();

                    if (_endFastMove != null)
                        _endFastMove.Clear();

                    if (_currentFastMove != null)
                        _currentFastMove.Clear();

                    _firstFastMove = null;
                    _endFastMove = null;
                    _currentFastMove = null;
                    _listCount = 0;
                    //Debug.LogError("1");
                    return true;
                }
                else // 第一个对象执行完，后置有对象
                {
                    if (_firstFastMove != null)
                    {
                        _firstFastMove = _currentFastMove.Next;
                        _firstFastMove.Previous = null;
                        _currentFastMove.Clear();
                        _currentFastMove = _firstFastMove;
                    }
                    //Debug.LogError("2");
                }
            }
            else 
            {
                // 执行到最后一个对象
                if (_currentFastMove.Next == null)
                {
                    _endFastMove = _currentFastMove.Previous;
                    _endFastMove.Next = null;
                    _currentFastMove.Clear();
                    _currentFastMove = _firstFastMove;
                    //Debug.LogError("3");
                    _listCount -= 1;
                    return true;
                }
                else // 继续执行下一个对象
                {
                    _tempFastMove = _currentFastMove.Next;
                    _tempFastMove.Previous = _currentFastMove.Previous;
                    _tempFastMove.Previous.Next = _tempFastMove;
                    _currentFastMove.Clear();
                    _currentFastMove = _tempFastMove;
                    _tempFastMove = null;
                    //Debug.LogError("4");
                }
            }

            _listCount -= 1;
            return false;
        }

        public void actionBegin(FastMove fm, 
            int renderQueue, 
            float startX, 
            float startY, 
            float startZ, 
            float endX, 
            float endY, 
            float endZ,
            int duration, 
            int delay, 
            bool world, 
            bool noEvt)
        {
            if (fm == null)
                return;

            fm.SetParams(duration, delay, world, noEvt);
            if (renderQueue != 9999)
            {
                fm.SetRenderQ(renderQueue);
            }
            
            if (startX != 9999 && startY != 9999 && startZ != 9999)
            {
                fm.SetStartPos(startX, startY, startZ);
            }
            else
            {
                fm.SetStartPos();
            }
            fm.SetEndPos(endX, endY, endZ);
            Begin(fm);
        }

        // { PreCnt = preCnt, active = { Count = 0 }, deactive = { Count = 0 } }
        public void groupActionBegin(LuaTable actionParams, int count)
        {
            if (count <= 0)
                return;

            FastMoveManager.RenderIndex = FastMoveManager.RenderIndex + 1;
            LuaTable act = null;
            for (int i=1; i<=count; i++)
            {
                act = (LuaTable)actionParams[i];
                FastMove a1 = act.toObject(1) as FastMove; //act[1] as FastMove;
                int a2      = act.toInt32(2);   //(int)((double)act[2]);
                float a3    = act.toFloat(3);      //(float)((double)act[3]);
                float a4    = act.toFloat(4); //(float)((double)act[4]);
                float a5    = act.toFloat(5); //(float)((double)act[5]);
                float a6    = act.toFloat(6); //(float)((double)act[6]);
                float a7    = act.toFloat(7); //(float)((double)act[7]);
                float a8    = act.toFloat(8); //(float)((double)act[8]);
                int a9      = act.toInt32(9); //(int)((double)act[9]);
                int a10     = act.toInt32(10);// (int)((double)act[10]);
                bool a11    = act.toBool(11);  //  (bool)act[11];
                bool a12    = act.toBool(12);
                actionBegin(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
            }
        }

    }
}

