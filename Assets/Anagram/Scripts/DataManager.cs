using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonClass<DataManager>
{
    const string dataPath = "Data/Anagram";
    const string imagePath = "Image/Anagram";

    public enum TYPE_LEVEL{EASY = 5, NORMAL = 8, HARD = 12, VHARD}

//    Dictionary<TYPE_LEVEL, List<AnagramData>> dataDic = new Dictionary<TYPE_LEVEL, List<AnagramData>>();
    List<AnagramData> dataDic = new List<AnagramData>();
    public DataManager()
    {
        initParse();
    }

    void initParse()
    {
        Sprite[] spriteList = Resources.LoadAll<Sprite>(imagePath);

        TextAsset textAsset = Resources.Load(dataPath) as TextAsset;

        if (textAsset != null)
        {
            string[] records = textAsset.text.Split('\n');
            foreach (string record in records)
            {
                string[] words = record.Split('\t');

                if (words.Length < 2) continue;

                string answer = words[0].Trim();
                string question = words[1].Trim();


                Sprite sprite = null;
                foreach (Sprite spr in spriteList)
                {
                    if (spr.name == answer)
                    {
                        sprite = spr;
                        break;
                    }
                }

                if (question != "" && answer != "" && sprite != null)
                {
                    AnagramData data = new AnagramData(question, answer, sprite);
                    TYPE_LEVEL typeLevel = TYPE_LEVEL.EASY;
//                    TYPE_LEVEL typeLevel = TYPE_LEVEL.VHARD;
                    //if (answer.Length < (int)TYPE_LEVEL.EASY)
                    //    typeLevel = TYPE_LEVEL.EASY;
                    //else if (answer.Length < (int)TYPE_LEVEL.NORMAL)
                    //    typeLevel = TYPE_LEVEL.NORMAL;
                    //else if (answer.Length < (int)TYPE_LEVEL.HARD)
                    //    typeLevel = TYPE_LEVEL.HARD;

                    AddDictionary(typeLevel, data);
                }
                else
                {
                    Debug.LogErrorFormat("Data is Not Found : Q{0} / A{1} / S{2}", question, answer, sprite);
                }
            }
        }
        else
        {
            Debug.LogErrorFormat("{0} is not Found", dataPath);
        }
    }

    void AddDictionary(TYPE_LEVEL typeLevel, AnagramData data)
    {
        dataDic.Add(data);
        //if (!dataDic.ContainsKey(typeLevel))
        //    dataDic.Add(typeLevel, new List<AnagramData>());
        //dataDic[typeLevel].Add(data);
    }

    /// <summary>
    /// 데이터 랜덤 가져오기
    /// </summary>
    /// <param name="typeLevel"></param>
    /// <returns></returns>
    public AnagramData GetData(TYPE_LEVEL typeLevel)
    {
        if (dataDic.Count > 0)
        {
            int range = dataDic.Count;
            if (range > 0)
            {
                return dataDic[UnityEngine.Random.Range(0, range)];
            }
            else
            {
                Debug.LogErrorFormat("{0} is Empty", typeLevel);
                return null;
            }
        }

        //if (dataDic.ContainsKey(typeLevel))
        //{
        //    int range = dataDic[typeLevel].Count;
        //    if (range > 0)
        //    {
        //        return dataDic[typeLevel][UnityEngine.Random.Range(0, range)];
        //    }
        //    else
        //    {
        //        Debug.LogErrorFormat("{0} is Empty", typeLevel);
        //        return null;
        //    }
        //}

        Debug.LogErrorFormat("{0} is not Initialize", typeLevel);
        return null;
    }
}
