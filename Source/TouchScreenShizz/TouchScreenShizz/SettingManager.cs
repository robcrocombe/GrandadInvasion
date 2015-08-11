using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GranddadInvasionNS
{
    public enum Setting
    {
        GranddadDeathSounds,
        SoundEffects,
        Vibration,
        Confetti,
        None
    }

    public class SettingManager
    {
        static List<SettingObject> settings = new List<SettingObject>();

        public static void setUpSettings()
        {
            settings.Add(new SettingObject(Setting.GranddadDeathSounds, true));
            settings.Add(new SettingObject(Setting.SoundEffects, true));
            settings.Add(new SettingObject(Setting.Vibration, true));
            settings.Add(new SettingObject(Setting.Confetti, false));
            saveSettings();
        }

        public static void loadSettings()
        {
            UnordereredSettings unorderedSettings = new UnordereredSettings();

            unorderedSettings.Load();

            settings = unorderedSettings;
        }

        public static void saveSettings()
        {
            UnordereredSettings unorderedSettings = new UnordereredSettings();
            foreach (SettingObject setting in settings)
            {
                unorderedSettings.Add(setting);
            }
            unorderedSettings.Save();
        }

        public static bool getSetting(Setting setting)
        {
            loadSettings();
            foreach (SettingObject settingObject in settings)
            {
                if (settingObject.SettingName == setting)
                {
                    return settingObject.SettingChoice;
                }
            }
            return false;
        }

        public static void changeSetting(Setting setting)
        {
            foreach (SettingObject settingObject in settings)
            {
                if (settingObject.SettingName == setting)
                {
                    settingObject.SettingChoice = !settingObject.SettingChoice;
                    break;
                }
            }
            saveSettings();
        }

        private class UnordereredSettings : List<SettingObject>
        {
            private const string XMLDocumentLocation = "settings.xml";
            public void Load()
            {
                XDocument settingsXML = IsolatedStorageSystem.loadXDocument(XMLDocumentLocation);

                var query = from xElem in settingsXML.Descendants("setting")
                            select new SettingObject
                            {
                                SettingName = SettingObject.getSettingType((string)xElem.Element("settingName").Value),
                                SettingChoice = Boolean.Parse(xElem.Element("settingChoice").Value),
                            };
                this.Clear();
                AddRange(query);
            }

            public void Save()
            {
                XElement settingsXML = new XElement("settings",
                                            from s in this
                                            select new XElement("setting",
                                                    new XElement("settingName", s.SettingName.ToString()),
                                                    new XElement("settingChoice", s.SettingChoice.ToString())));

                IsolatedStorageSystem.saveXDocument(XMLDocumentLocation, settingsXML);
            }
        }
    }

    public class SettingObject
    {
        public Setting SettingName;
        public bool SettingChoice;

        public SettingObject(Setting settingName, bool settingChoice)
        {
            SettingName = settingName;
            SettingChoice = settingChoice;
        }
        public SettingObject()
        {
            //Empty constructor for linq
        }

        public static Setting getSettingType(string SettingString)
        {
            switch (SettingString)
            {
                case "GranddadDeathSounds":
                    return Setting.GranddadDeathSounds;
                case "SoundEffects":
                    return Setting.SoundEffects;
                case "Confetti":
                    return Setting.Confetti;
                case "Vibration":
                    return Setting.Vibration;
                default:
                    return Setting.None;
            }
        }
    }

    public class OnOffSlider
    {
        bool Selection;
        public string Name;

        public static Texture2D offImage;
        public static Texture2D onImage;
        public static Texture2D themeColourOverlay;

        public SpriteObject sprite;

        public void Draw(SpriteBatch sb)
        {
            if (Selection == true)
            {
                sb.Draw(onImage, sprite.spriteRectangle, Color.White);
            }
            else
            {
                sb.Draw(offImage, sprite.spriteRectangle, Color.White);
            }
        }

        public OnOffSlider(SpriteObject spriteObj, bool defaultState)
        {
            sprite = spriteObj;
            switch (defaultState)
            {
                case true:
                    sprite.spriteTexture = onImage;
                    break;
                case false:
                    sprite.spriteTexture = offImage;
                    break;
            }
            Selection = defaultState;
        }

        public bool switchChoice()
        {
            if (Selection == true)
            {
                Selection = false;
            }
            else
            {
                Selection = true;
            }
            return Selection;
        }

        public bool IsTapped()
        {
            TouchCollection touches = TouchPanel.GetState();

            for (int i = 0; i < touches.Count; i++)
            {
                Rectangle fingerPosition = new Rectangle((int)touches[i].Position.X, (int)touches[i].Position.Y, 1, 1);
                if (fingerPosition.Intersects(this.sprite.spriteRectangle))
                {
                    return true;
                }
            }
            return false;
        }
    }
}