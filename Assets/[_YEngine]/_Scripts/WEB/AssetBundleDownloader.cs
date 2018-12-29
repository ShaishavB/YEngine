using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;

namespace YEngine.WEB
{
    public class AssetBundleDownloader
    {
        private AssetBundle _bundle;
        private GameObject _bundlePrefab;
        private bool _isDone;
        private Stream _fileStream;

        private UnityWebRequest _uwr;
        public bool isDone
        {
            get
            {
                return _isDone;
            }
        }

        public AssetBundle currentBundle
        {
            get
            {
                return _bundle;
            }
        }

        public GameObject bundlePrefab
        {
            get
            {
                return _bundlePrefab;
            }
        }

        public UnityWebRequest request
        {
            get
            {
                return _uwr;
            }
        }

        public void UnloadBundle(AssetBundle bundle)
        {
            if (bundle != null)
            {
                bundle.Unload(true);
            }

        }

        public void LoadBundleFromLocal(string url, string KeyWithExtention = null)
        {
            //Create a mapper for path and version later
            string path = Path.Combine(Application.persistentDataPath, KeyWithExtention);
            UnityEngine.Debug.Log(path);
            _isDone = false;
            //Unload bundle if any loaded before load new bundle 
            if (_bundle != null)
            {
                _bundle.Unload(true);
            }

            _bundlePrefab = null;
            if (!File.Exists(path))
            {
                //Create and download new asset bundle and set the info in binary
                //BundleDataHandler.Instance.AddItem(path, version, TAG);
                UnityEngine.Debug.Log("Bundle is not available");
            }
            else
            {
                //bundle already downloaded
                //just need to load from the local path
                _bundle = AssetBundle.LoadFromFile(path);
                UnityEngine.Debug.Log("loaded->" + _bundle.name);
                string[] allAssets = _bundle.GetAllAssetNames();
                foreach (string nm in allAssets)
                {
                    UnityEngine.Debug.Log(nm);
                    _bundlePrefab = _bundle.LoadAsset<GameObject>(nm);
                }
                _isDone = true;
            }
        }
        public IEnumerator UpdateInLocal(string url, int newversion, string KeyWithExtention = null)
        {
            yield break;
        }


        public IEnumerator DownloadInLocal(string url, int version, string KeyWithExtention = null)
        {
            //Create a mapper for path and version later
            string path = Path.Combine(Application.persistentDataPath, KeyWithExtention);
            UnityEngine.Debug.Log(path);
            _isDone = false;
            //Unload bundle if any loaded before load new bundle 
            if (_bundle != null)
            {
                _bundle.Unload(true);
            }

            _bundlePrefab = null;
            bool hasTodownload = false;
            if (!File.Exists(path))
            {
                //Create and download new asset bundle and set the info in binary
                //BundleDataHandler.Instance.AddItem(path, version, TAG);
                hasTodownload = true;
            }
            else
            {
                //check the version from binary and map with the current version 
                //if saved version < new version 
            }

            if (hasTodownload)
            {
                _uwr = UnityWebRequest.Get(url);
                yield return _uwr.SendWebRequest();
                if (_uwr.isNetworkError || _uwr.isHttpError)
                {
                    Debug.Log("Download Error");
                    Debug.Log(_uwr.error);
                }
                else
                {
                    //Check and delete if old version is already stored in local
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    //Save downloaded data in local space
                    if (_fileStream == null)
                    {
                        _fileStream = File.OpenWrite(path);
                    }
                    // write data to file
                    _fileStream.Write(_uwr.downloadHandler.data, 0, _uwr.downloadHandler.data.Length);
                    yield return new WaitForSeconds(3);
                    Debug.Log("Saved");
                    _fileStream.Close();
                    _bundle = AssetBundle.LoadFromFile(path);
                    UnityEngine.Debug.Log("loaded->" + _bundle.name);
                    string[] allAssets = _bundle.GetAllAssetNames();
                    foreach (string nm in allAssets)
                    {
                        UnityEngine.Debug.Log(nm);
                        _bundlePrefab = _bundle.LoadAsset<GameObject>(nm);
                    }
                    _isDone = true;
                    //set old version = new version in binary and save data
                }
            }
            else
            {
                //bundle already downloaded
                //just need to load from the local path
                _bundle = AssetBundle.LoadFromFile(path);
                UnityEngine.Debug.Log("loaded->" + _bundle.name);
                string[] allAssets = _bundle.GetAllAssetNames();
                foreach (string nm in allAssets)
                {
                    UnityEngine.Debug.Log(nm);
                    _bundlePrefab = _bundle.LoadAsset<GameObject>(nm);
                }
                _isDone = true;
            }
        }

    }


    class LocalDownloadHandler : DownloadHandlerScript
    {
        private string _targetFilePath;
        private Stream _fileStream;

        public float progress;

        public LocalDownloadHandler(string targetFilePath) : base(new byte[4096])
        {
            _targetFilePath = targetFilePath;
        }

        protected override bool ReceiveData(byte[] data, int dataLength)
        {

            Debug.Log("DataLength :" + dataLength);

            // create or open target file
            if (_fileStream == null)
            {
                _fileStream = File.OpenWrite(_targetFilePath);
            }
            // write data to file
            _fileStream.Write(data, 0, dataLength);
            return true;
        }

        protected override void CompleteContent()
        {
            // close and save
            _fileStream.Close();
        }
    }
}
