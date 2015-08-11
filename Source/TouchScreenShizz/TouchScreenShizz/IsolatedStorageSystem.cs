using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;

namespace GranddadInvasionNS
{
    class IsolatedStorageSystem
    {
        private static IsolatedStorageFile isoStore;

        public static void Initalize()
        {
             isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        }

        public static bool CheckFileExist(string location)
        {
            if(isoStore.FileExists(location))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static XDocument loadXDocument(string location)
        {
            XDocument XML;
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(location, FileMode.Open, isoStore))
            {
                XML = XDocument.Load(isoStream);
            }
            return XML;
        }

        public static void saveXDocument(string location, XDocument XML)
        {
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(location, FileMode.Create, isoStore))
            {
                XML.Save(isoStream);
            }
        }

        public static void saveXDocument(string location, XElement XML)
        {
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(location, FileMode.Create, isoStore))
            {
                XML.Save(isoStream);
            }
        }
    }
}
