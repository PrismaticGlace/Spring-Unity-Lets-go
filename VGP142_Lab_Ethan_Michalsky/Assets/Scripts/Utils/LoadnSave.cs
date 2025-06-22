using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class LoadnSave : MonoBehaviour {
    [XmlRoot("GameData")]
    public class GameStateData {
        public struct TransformData {
            public float posX;
            public float posY;
            public float posZ;
            public float rotX;
            public float rotY;
            public float rotZ;
            public float scaleX;
            public float scaleY;
            public float scaleZ;
        }

        //Player Data
        public class PlayerData {
            public TransformData posRotSca;
            public bool collectWeapon;
            public float playerHealth;
        }
        public PlayerData playerData = new PlayerData();
    }

    public GameStateData gameState = new GameStateData();

    public void Save(string fileName = "GameData.xml") {
        XmlSerializer serlizer = new XmlSerializer(typeof(GameStateData));
        FileStream stream = new FileStream(fileName, FileMode.Create);
        serlizer.Serialize(stream, gameState);
        stream.Flush();
        stream.Dispose();
        stream.Close();
    }

    public void Load(string fileName = "GameData.xml") {
        XmlSerializer serlizer = new XmlSerializer(typeof(GameStateData));
        FileStream stream = new FileStream(fileName, FileMode.Open);
        gameState = serlizer.Deserialize(stream) as GameStateData;
        stream.Flush();
        stream.Dispose();
        stream.Close();
    }
}
