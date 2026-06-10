using System;

public static class JsonHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    public static T[] FromJson<T>(string json)
    {
        string wrappedJson = "{\"Items\":" + json + "}";
        return UnityEngine.JsonUtility.FromJson<Wrapper<T>>(wrappedJson).Items;
    }
}