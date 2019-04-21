using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

public class ProtoBase {
	
	protected static List<T> LoadPoto<T>(string filename) where T : class, IExtensible
	{
			TextAsset text = (TextAsset)Resources.Load(GameDefine.ProtoPath + filename);
	        if (text != null)
	        {
	            MemoryStream stream = new MemoryStream(text.bytes);
	            List<T> data = Serializer.Deserialize<List<T>>(stream);
	            return data;
	        }
	        else
	        {
	            Debug.LogError(filename + " is not exit!!! ");
	            return new List<T>();
	        }
		
	}
}
