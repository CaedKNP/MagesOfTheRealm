using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using Assets.Resources.Entities;

namespace Assets._Scripts.Managers
{
    /// <summary>
    /// Manages I/O of scores using xml serialization
    /// </summary>
    public class XMLManager : StaticInstance<XMLManager>
    {
        void Start()
        {
            if (!Directory.Exists(Application.persistentDataPath + "/HighScores/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/HighScores/");
            }
        }

        /// <summary>
        /// Serialies given list of scores and saves them to the file
        /// </summary>
        /// <param name="scoresToSave">list of scores that you want to save</param>
        public void SaveScores(List<HighScore> scoresToSave)
        {
            XmlSerializer serializer = new(typeof(List<HighScore>));
            FileStream stream = new(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Create);
            serializer.Serialize(stream, scoresToSave);
            stream.Close();
        }

        /// <summary>
        /// Loading list of hight scores
        /// </summary>
        /// <returns>list of hight scores</returns>
        public List<HighScore> LoadScores()
        {
            if (File.Exists(Application.persistentDataPath + "/HighScores/highscores.xml"))
            {
                XmlSerializer serializer = new(typeof(List<HighScore>));
                FileStream stream = new(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Open);

                return serializer.Deserialize(stream) as List<HighScore>;
            }
            else
                return new List<HighScore>();
        }

        protected override void OnApplicationQuit()
        {
            var scoresToSave = GameManager.Instance.GetHightScores();
            SaveScores(scoresToSave);
            base.OnApplicationQuit();
        }
    }
}