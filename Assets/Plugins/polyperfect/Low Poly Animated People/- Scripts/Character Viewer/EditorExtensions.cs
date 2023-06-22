using System.IO;
using UnityEngine;

namespace Polyperfect.People
{

    public static class EditorExtensions
    {

        public static void SaveTexture(Sprite sprite, string path, string name)
        {
            byte[] bytes = sprite.texture.EncodeToPNG();
            File.WriteAllBytes(path + "/" + name + ".png", bytes);
        }

        //Causing bugs in newer versions of unity
        /*
        [ContextMenu("Get Path")]
        public static string GetPath()
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = getActiveFolderPath.Invoke(null, new object[0]);
            return obj.ToString();
        }
        */
    }
}
