using UnityEngine;
using System.IO; //allows us to work with files and directories
using System.Xml.Serialization; //allows us to serialize and deserialize xml files
//serialize = translating data from one format to another
//Transforming it in and out of formats that Unity is capable of working with.

public class GameUtility
{
    public const float ResolutionDelayTime = 1; //display resolution screen for 1 second
    public const string SavePrefKey = "Game_Highscore_Value"; //saves the highscore value
    public const string xmlFileName = "Questions.xml"; //xml file name, which file to load
    public static string xmlFilePath = Application.dataPath + "/" + xmlFileName; //xml file's file path
}

[System.Serializable()] //marks this Data class as serializable, if this wasn't marked we won't
//be able to save this to the question xml file
public class Data
{
    public Question[] Questions = new Question[0]; //list of Question objects
    public static void Write(Data data) //write this data into the xml file
    {
        XmlSerializer  serializer = new XmlSerializer(typeof(Data)); //serialize Data class, transform data into a format Unity can use, store the object
        using (Stream stream = new FileStream(GameUtility.xmlFileName, FileMode.Create)) //using the file path, it creates data in that file
        {
            serializer.Serialize(stream, data);
        }
    }
    public static Data Fetch() //overloaded
    {
        return Fetch(out bool result); //references the other Fetch function
    }
    public static Data Fetch(out bool result) //fetch the data
    //out keyword is used when method returns multiple values
    {
        if (!File.Exists(GameUtility.xmlFilePath)) //if the filepath goes to a file that doesn't exist
        {
            result = false;
            return new Data(); //then return new Data()
        }
        
        XmlSerializer deserializer = new XmlSerializer(typeof(Data)); //we are going to deserialize from the xml to this data class, get the stored object from a certain location
        using (Stream stream = new FileStream(GameUtility.xmlFilePath, FileMode.Open)) //using the file path, open this file
        {
            var data = (Data)deserializer.Deserialize(stream); //stream is accessing this xml file, once it accesses this file, it opens the file
            //stores data into local data variable
            //cast this object to be a Data object
            result = true;
            return data; //returns the data
        }
    }
}