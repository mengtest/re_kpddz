using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FastAction
{
    [SLua.CustomLuaClass]
    public class FastMove : MonoBehaviour
    {
        // 链表数据
        public FastMove Previous = null;
        public FastMove Next = null;

        public delegate void OnMoveEvent(FastMove fMove);
        public OnMoveEvent OnDefaultEvent = null;
        public OnMoveEvent OnOtherEvent = null;
        public bool IsOtherEvent = false;
        public bool NoEvent = false;
        public int Duration = 0;
        public int Delay = 0;
        public Vector3 StartPos;
        public Vector3 EndPos;
        public bool IsWorld = false;

        public bool _doingAction = false;

        private Vector3 _tempPos;
        private float _speed = 0f;
        private float _delay = 0f;
        private bool _isInit = false;

        private bool _isGold = false;
        public int _renderQueue = 3000;
        private Material _mat = null;

        private bool _stopManual = false;

        //test
        //public int index = 1000;
        public int name = 0;
        public double time = 0;

        public void Start()
        {
            _isGold = gameObject.name.Contains("goldPrefab");
            if (_isGold)
            {
                //_mat = gameObject.GetComponent<MeshRenderer>().material;
            }
        }

        public void Stop()
        {
            _stopManual = true;
        }
           

        void OnDestroy()
        {
            if (name != 0)
            {
                //Debug.Log(name + " is Destory");
            }
            OnDefaultEvent = null;
            OnOtherEvent = null;
            Next = null;
            Previous = null;
            _mat = null;
        }

        public void Clean()
        {
            //Debug.Log(name + " is clean");
            if (Next != null) 
                Next.Clean();
            Clear();
            
        }

        public void Clear()
        {
            Next = null;
            Previous = null;
        }

        public void Prepare()
        {
            float dis = Vector3.Distance(StartPos, EndPos);
            _speed = dis / (Duration * 0.001f);
            _delay = Delay * 0.001f;
            _isInit = false;
            _doingAction = true;
            _stopManual = false;
        }

        public void SetParams(int duration, int delay, bool world, bool noEvt)
        {
            Duration = duration;
            Delay = delay;
            IsWorld = world;
            NoEvent = noEvt;
        }

        public void SetRenderQ(int render)
        {
            _renderQueue = render;
        }

        public void SetPositionToStartPos()
        {
            if (IsWorld == true)
            {
                transform.position = StartPos;
            }
            else
            {
                transform.localPosition = StartPos;
            }
        }

        private void initPosition()
        {
            SetPositionToStartPos();
            _isInit = true;
        }

        public void SetDefaultCall(OnMoveEvent evt)
        {
            if (OnDefaultEvent != null)
            {
                OnDefaultEvent = null;
            }
            OnDefaultEvent = evt;
        }

        public void SetOtherCall(OnMoveEvent evt)
        {
            if (OnOtherEvent != null)
            {
                OnOtherEvent = null;
            }
            OnOtherEvent = evt;
        }

        public void SetStartPos(float x, float y, float z)
        {
            StartPos.x = x;
            StartPos.y = y;
            StartPos.z = z;
        }

        public void SetEndPos(float x, float y, float z)
        {
            EndPos.x = x;
            EndPos.y = y;
            EndPos.z = z;
        }

        public void SetStartPos()
        {
            if (IsWorld == true)
            {
                StartPos = transform.position;
            }
            else
            {
                StartPos = transform.localPosition;
            }
        }

        public void SetEndPos()
        {
            if (IsWorld == true)
            {
                EndPos = transform.position;
            }
            else
            {
                EndPos = transform.localPosition;
            }
        }

        private bool positionEqual(Vector3 a, Vector3 b) 
        {
            if (!Utils.LocalMathf.Approximately(a.x, b.x) || !Utils.LocalMathf.Approximately(a.y, b.y) || !Utils.LocalMathf.Approximately(a.z, b.z))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool moveFunc(Vector3 pos, float delta)
        {
            if (IsWorld == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, _speed * delta);
                return positionEqual(transform.position, pos);
            }
            else
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, pos, _speed * delta);
                return positionEqual(transform.localPosition, pos);
            }   
        }

        public bool Doing(float delta)
        {
            if (_isInit == false)
            {
                if (_delay > 0)
                {
                    _delay = _delay - delta;
                    return true;
                }
                else
                {
                    _delay = 0;
                    initPosition();
                }

                if (_isGold)
                {
                    //_renderQueue = _mat.renderQueue;
                    //_mat.renderQueue = _renderQueue + 30;
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, FastMoveManager.RenderIndex);
                    EndPos.z = FastMoveManager.RenderIndex;
                }
            }

            if (_stopManual || moveFunc(EndPos, delta))
            {
                //Debug.Log(name + " is over -> " + (UtilTools.GetCurrentTime() - time));
                if (!NoEvent)
                {
                    if (IsOtherEvent && OnOtherEvent != null)
                    {
                        OnOtherEvent(this);
                    }
                    else if (!IsOtherEvent && OnDefaultEvent != null)
                    {
                        OnDefaultEvent(this);
                    }
                }

                if (_isGold)
                {
                    //_mat.renderQueue = _renderQueue;
                }

                _doingAction = false;
                return false;
            }
            else
            {
                return true;
            }
        }

        public void pause()
        {

        }

        public void end()
        {

        }
    }
}
