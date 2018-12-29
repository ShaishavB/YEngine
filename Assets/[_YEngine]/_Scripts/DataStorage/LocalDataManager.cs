using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

namespace YEngine
{
    public class LocalDataManager : MonoBehaviour
    {
        private LocalDataManager _localData;

        public LocalDataManager Inst
        {
            get
            {
                if (_localData != null)
                {
                    _localData = new LocalDataManager();
                }

                return _localData;
            }
        }

        public T Load<T>(T param, string Key)
        {
            string path = Application.persistentDataPath + "/" + Key + ".bin";
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            if (File.Exists(path))
            {
                param = (T)bf.Deserialize(fs);
            }
            else
            {
                bf.Serialize(fs, param);
            }
            fs.Close();
            return param;
        }

        public T Save<T>(T param, string Key)
        {
            string path = Application.persistentDataPath + "/" + Key + ".bin";
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, param);
            fs.Close();
            return param;
        }
    }


    [Serializable]
    public class UserData
    {
        public string Name;
        public string EmailId;
        public UserData() { }
    }

    [Serializable]
    public class ShopData
    {
        public List<ProductData> products;
        public ShopData() { }
    }


    [Serializable]
    public class ProductData
    {
        public string Id;
        public string Name;
    }

}