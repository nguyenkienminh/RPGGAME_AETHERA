using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class FileDataHandle 
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool encryptData = false;
    private string codeWord = "@Sktt9123@";
    public FileDataHandle(string _dataDirPath,string _dataFileName,bool _enCryptData)
    {
        dataDirPath = Application.persistentDataPath;
        dataFileName = _dataFileName;
        encryptData = _enCryptData;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonConvert.SerializeObject(_data, Formatting.Indented);

            foreach (var skill in _data.skillTree)
            {
                Debug.Log($"🔹 Skill ID: {skill.Key}, Unlocked: {skill.Value}");
            }

            Debug.Log("📝 Lưu file JSON: " + dataToStore);

            //if(encryptData)
            //{
            //    dataToStore = EncryptDecrypt(dataToStore);
            //}

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath,dataFileName);
        Debug.Log("📖 Đọc file JSON thành công: " + fullPath);

        GameData loadData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //if (encryptData)
                //{
                //    dataToLoad = EncryptDecrypt(dataToLoad);
                //}

                Debug.Log("📖 Dữ liệu2: " + dataToLoad);
                loadData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
                foreach (string id in loadData.equipmentId)
                {
                    Debug.Log("🎮 Equipment ID: " + id);
                }
                return loadData;
            }
            catch(Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        else
        {
            Debug.LogWarning("⚠ File JSON không tồn tại, tạo file mới...");
            Save(new GameData());
            
        }
        return new GameData();
    }
    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    private string EncryptDecrypt(string _data)
    {
        string modifierData = "";
        for(int i = 0; i < _data.Length; i++)
        {
            modifierData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);
        }
        return modifierData;
    }
}
