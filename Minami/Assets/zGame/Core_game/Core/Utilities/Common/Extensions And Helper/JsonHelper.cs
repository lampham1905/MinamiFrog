﻿/***
 * Created by HungNB - NBEAR
 * 
 * Usage:
 * YouObject[] objects = JsonHelper.GetJsonArray<YouObject> (jsonString);
 * string jsonString = JsonHelper.ArrayToJson<YouObject>(objects);
 **/

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper
{
    public static class JsonHelper
    {
        public static T[] GetJsonArray<T>(string json)
        {
            try
            {
                var sb = new StringBuilder();
                string newJson = sb.Append("{").Append("\"array\":").Append(json).Append("}").ToString();
                Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
                return wrapper.array;
            }
            catch
            {
                Debug.Debug.LogError((typeof(T).Name));
                return null;
            }
        }

        public static List<T> GetJsonList<T>(string json)
        {
            try
            {
                var sb = new StringBuilder();
                string newJson = sb.Append("{").Append("\"list\":").Append(json).Append("}").ToString();
                ListWrapper<T> wrapper = JsonUtility.FromJson<ListWrapper<T>>(newJson);
                return wrapper.list;
            }
            catch
            {
                Debug.Debug.LogError((typeof(T).Name));
                return null;
            }
        }

        public static string ArrayToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.array = array;
            string json = JsonUtility.ToJson(wrapper);
            json = json.Remove(0, 9);
            json = json.Remove(json.Length - 1);
            return json;
        }

        public static string ListToJson<T>(List<T> list)
        {
            ListWrapper<T> wrapper = new ListWrapper<T>();
            wrapper.list = list;
            string json = JsonUtility.ToJson(wrapper);
            json = json.Remove(0, 8);
            json = json.Remove(json.Length - 1);
            return json;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }

        [Serializable]
        private class ListWrapper<T>
        {
            public List<T> list;
        }
    }
}