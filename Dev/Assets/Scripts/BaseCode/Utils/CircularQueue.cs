using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CircularQueue<T>
{
    protected T[] _elements;
    int _head;
    int _tail;
    int _len;

    public CircularQueue(int len = 32)
    {
        _elements = new T[len];
        _head = 0;
        _tail = 0;
        _len = len;
    }

    public int Count
    {
        get
        {
            if (_tail >= _head)
                return _tail - _head;
            else
                return _tail + _len - _head;
        }
    }

    public T At(int i)
    {
        if (Count <= i)
        {
            throw new Exception("访问超界");
        }

        int idx = _head + i;
        if (idx >= _len)
            idx -= _len;
        return _elements[idx];
    }

    public void PushFront(T t)
    {
        _head -= 1;
        if (_head < 0)
        {
            _head += _len;
        }
        _elements[_head] = t;

        CheckAndDoubleArray();
    }

    public void PushBack(T t)
    {
        _elements[_tail] = t;
        _tail += 1;
        if (_tail >= _len)
        {
            _tail -= _len;
        }

        CheckAndDoubleArray();
    }

    public T PopFront()
    {
        if (Count == 0)
        {
            throw new Exception("队列为空");
        }
        T t = _elements[_head];
        _elements[_head] = default(T);

        _head += 1;
        if (_head >= _len)
        {
            _head -= _len;
        }
        return t;
    }

    public T PopBack()
    {
        if (Count == 0)
        {
            throw new Exception("队列为空");
        }
        _tail -= 1;
        if (_tail < 0)
        {
            _tail += _len;
        }
        T t = _elements[_tail];
        _elements[_tail] = default(T);

        return t;
    }

    public T Front
    {
        get
        {
            if (Count == 0)
            {
                throw new Exception("队列为空");
            }
            return _elements[_head];
        }
    }

    public void CheckAndDoubleArray()
    {
        if (_head != _tail)
        {
            return;
        }

        T[] newElements = new T[_len + _len];
        int idx = 0;
        for (int i = _head; i < _len; ++i)
        {
            newElements[idx] = _elements[i];
            ++idx;
        }

        for (int i = 0; i < _tail; ++i)
        {
            newElements[idx] = _elements[i];
            ++idx;
        }

        _head = 0;
        _tail = _len;
        _len += _len;
        _elements = newElements;
    }

    public void Clear()
    {
        for (int i = _head; i < _len; ++i)
        {
            _elements[i] = default(T);
        }

        for (int i = 0; i < _tail; ++i)
        {
            _elements[i] = default(T);
        }

        _head = 0;
        _tail = 0;
    }
}

