using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using Assets.Resources.Entities;

namespace Assets._Scripts.Managers
{
    public class XMLManager : StaticInstance<XMLManager>
    {
        void Start()
        {
            if (!Directory.Exists(Application.persistentDataPath + "/HighScores/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/HighScores/");
            }
        }

        public void SaveScores(List<HighScore> scoresToSave)
        {
            XmlSerializer serializer = new(typeof(HighScore));
            FileStream stream = new(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Create);
            serializer.Serialize(stream, scoresToSave);
            stream.Close();
        }

        public List<HighScore> LoadScores()
        {
            if (File.Exists(Application.persistentDataPath + "/HighScores/highscores.xml"))
            {
                XmlSerializer serializer = new(typeof(HighScore));
                FileStream stream = new(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Open);
                return serializer.Deserialize(stream) as List<HighScore>;
            }
            return new List<HighScore>();
        }
    }
}