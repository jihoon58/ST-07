using UnityEngine;
using System;
using System.Collections.Generic;

namespace ST07.Data{
    /// <summary>
    /// 건물 데이터 클래스
    /// </summary>
    public class Building
    {
        [Serializable]
        public class Type{
            public string type;
            public List<Index> indexList;
        }

        [Serializable]
        public class Index{
            public int index;
            public List<Item> itemList;
        }
        
        [Serializable]
        public class Item
        {
            [Tooltip("아이템 이름")]
            public string itemName;
            [Tooltip("아이템 개수")]
            public int itemCount;
            [Tooltip("아이템 위치 (x, y)")]
            public Vector2 itemPos;
        }

        [Serializable]
        public class BuildingData
        {
            public List<Type> typeList;
            public List<Item> getItemList(string type, int index){
                foreach (var t in typeList){
                    if (t.type == type){
                        foreach (var i in t.indexList){
                            if (i.index == index){
                                return i.itemList;
                            }
                        }
                    }
                }
                return null;
            }
        }
    }
}