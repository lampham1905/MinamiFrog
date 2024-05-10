using System;
using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class EventObject
    {
        public EventCMD cmd;
        public Action<object> callBack;
        public EventObject(EventCMD c, Action<object> cal)
        {
            cmd = c;
            callBack = cal;
        }
    }

    public class RaiseEventObject
    {
        public EventCMD cmd;
        public object obj;
        public RaiseEventObject(EventCMD c, object o)
        {
            cmd = c;
            obj = o;
        }
    }

    public enum EventCMD
    {
      
        EVENT_POPUP_SHOW,
        EVENT_POPUP_CLOSE,
        
        
        SHOWADDBUILDING,
        SHOWBUILDING,
        MOVECAMERATOTARGET,
        BUILDBUILDING,
        HIDEADDBUILDING,
        SHOWSHOP,
        INITPATH,
        ADDPATH,
        SHOWUPGRADE,
        UPDATE_SATISFACTION,
        UPDATE_MONEY,
        UPDATE_VILLAGERS,
        UPDATE_BEAUTY,
        UPDATE_YOUTH,
        UPDATE_ELDER,
        UPDATE_GOLBIN,
        SHOW_BTN,
        HIDE_BTN,
        START_DAY,
        END_DAY,
        SPAWN_FROG
    }

    public static class ManagerEvent
    {
        static List<EventObject> listEvent = new List<EventObject>();
        public static void RegEvent(EventCMD cmd, Action<object> cal)
        {
            if (cal != null)
            {
                foreach (EventObject o in listEvent)
                    if (o.cmd == cmd && o.callBack == cal) return;
                listEvent.Add(new EventObject(cmd, cal));
            }
        }
        public static void RaiseEvent(EventCMD cmd, object obj = null)
        {
            for (int i = 0; i < listEvent.Count; ++i)
            {
                if (listEvent[i].cmd == cmd)
                    listEvent[i].callBack(obj);
            }
        }
        public static void RaiseEventNextFrame(EventCMD cmd, object obj = null)
        {
            ManagerGame.ListEvent.Add(new RaiseEventObject(cmd, obj));
        }
        public static void RemoveEvent(EventCMD cmd, Action<object> cal = null)
        {
            for (int i = 0; i < listEvent.Count; ++i)
            {
                if (listEvent[i].cmd == cmd)
                    if (cal == null || listEvent[i].callBack == cal)
                        listEvent.RemoveAt(i);
            }
        }
        public static void ClearEvent()
        {
            listEvent.Clear();
        }
    }
}