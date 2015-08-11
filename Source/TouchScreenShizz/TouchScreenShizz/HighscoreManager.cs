using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.IO.IsolatedStorage;

namespace GranddadInvasionNS
{
    class HighscoreManager
    {
        //Location of the highscores XML file
        const string XMLDocumentLocation = "highscores.xml";

        //List to put highscores in
        private static List<Highscore> highscores = new List<Highscore> { };

        //Last player added (used to add name, rather than no name)
        private static int lastPlayerAdded;

        public static void loadOrderedHighscores()
        {
            //List which will contain the highscores before they are ordered
            UnordereredHighscores unorderedList = new UnordereredHighscores();

            unorderedList.Load();

            //Sort the highscores by score
            unorderedList.Sort(new HighscoreComparer());

            //Put highscores into the highscores list, so it can be used outside this scope
            highscores = unorderedList;
        }

        public static void saveHighscores()
        {
            UnordereredHighscores unorderedList = new UnordereredHighscores();
            foreach (Highscore hs in highscores)
            {
                unorderedList.Add(hs);
            }
            unorderedList.Save();
        }

        //Method which allows any code to get a specific highscore
        public static Highscore getHighscore(int position)
        {
            if (position > 4)
            {
                throw new Exception("THERES ONLY 4 HIGHSCORES YOU STUPID CUNT");
            }

            loadOrderedHighscores();

            //Take 1 away from position because lists start at 0 rather than 1
            position--;

            //Get highscore and return it
            Highscore thisHighscore = highscores[position];
            return thisHighscore;
        }

        public static void setUpHighscores()
        {
            for (int i = 0; i <= 5; i++)
            {
                highscores.Add(new Highscore("Not Yet Played", DateTime.Now, 0));
                saveHighscores();
            }
        }

        //Method which allows any code to add a highscore, as long as they have a name and score
        public static int addHighscore(string Name, int Score)
        {
            for (int i = 0; i < highscores.Count; i++)
            {
                if (Score > highscores[i].Score)
                {
                    //Add highscore with this new score
                    highscores.Add(new Highscore(Name, DateTime.Now, Score));

                    //We need to remove the worst highscore
                    highscores.RemoveAt(3);

                    //Reorder the highscores
                    highscores.Sort(new HighscoreComparer());

                    saveHighscores();

                    lastPlayerAdded = i + 1;
                    return i + 1;
                }
            }
            lastPlayerAdded = 0;
            return 0;
        }

        //Overloaded method which allows code to add a highscore even if the user didnt want to provide a name
        public static int addHighscore(int score)
        {
            return addHighscore(DateTime.Now.ToString("H:mmtt - dd MMM"), score);
        }

        //Add name to the last value added
        public static void addNameToLastHighscoreAdded(string Name)
        {
            if (Name != null)
            {
                highscores[lastPlayerAdded - 1].Name = Name;
                saveHighscores();
            }
        }

        //Highscore Comparer Class, which tells the .Sort method of a Highscore list to sort by the score of the Highscore class
        private class HighscoreComparer : IComparer<Highscore>
        {
            public int Compare(Highscore highscore1, Highscore highscore2)
            {
                int returnValue = 1;
                if (highscore1 != null && highscore2 != null)
                {
                    returnValue = highscore2.Score.CompareTo(highscore1.Score);
                }
                return returnValue;

            }
        }

        //Highscore object
        public class Highscore
        {
            public string Name;
            public DateTime TimeAchieved;
            public int Score;

            public Highscore(string name, DateTime dt, int score)
            {
                Name = name;
                TimeAchieved = dt;
                Score = score;
            }
            public Highscore()
            {
                //Empty Constructor for Linq
            }
        }

        private class UnordereredHighscores : List<Highscore>
        {
            public void Load()
            {
                XDocument highscoresXML = IsolatedStorageSystem.loadXDocument(XMLDocumentLocation);

                var query = from xElem in highscoresXML.Descendants("highscore")
                            select new Highscore
                            {
                                Name = xElem.Element("playerName").Value,
                                TimeAchieved = DateTime.Parse(xElem.Element("dateAchieved").Value),
                                Score = int.Parse(xElem.Element("score").Value)
                            };
                this.Clear();
                AddRange(query);
            }

            public void Save()
            {
                XElement highscoresXML = new XElement("highscores",
                                            from h in this
                                            select new XElement("highscore",
                                                    new XElement("playerName", h.Name),
                                                    new XElement("dateAchieved", h.TimeAchieved),
                                                    new XElement("score", h.Score)));

                IsolatedStorageSystem.saveXDocument(XMLDocumentLocation, highscoresXML);
            }
        }
    }
}
