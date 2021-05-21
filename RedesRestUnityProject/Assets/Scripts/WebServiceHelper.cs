using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class WebServiceHelper
{
	public static IEnumerator Get(string url,
	                              Action<string> onResponse,
	                              Action<string> onError = null)
	{
		Debug.Log($"calling url (get): {url}");
		using (var request = UnityWebRequest.Get(url))
		{
			yield return request.SendWebRequest();
			if (request.isNetworkError
			    || request.isHttpError)
			{
				Debug.Log($"Url error: {url}\nError: {request.error}");
				onError?.Invoke(request.error);
				yield break;
			}

			Debug.Log($"Url response: {url}\nResponse: {request.downloadHandler.text}");
			onResponse(request.downloadHandler.text);
		}
	}

	public static IEnumerator Post(string url,
	                               Dictionary<string, string> fields,
	                               Action<string> onResponse,
	                               Action<string> onError = null)
	{
		var form = new WWWForm();
		Debug.Log($"calling url (post): {url}\nFields:" +
		          $" {fields.Aggregate(string.Empty, ((data, pair) => $"{data}\n{pair.Key}: {pair.Value}"))}");
		foreach (var field in fields)
		{
			form.AddField(field.Key, field.Value);
		}
		using (var request =
			UnityWebRequest.Post(url, form))
		{
			yield return request.SendWebRequest();
			if (request.isNetworkError
			    || request.isHttpError)
			{
				Debug.Log($"Url error: {url}\nError: {request.error}");
				onError?.Invoke(request.error);
				yield break;
			}

			Debug.Log($"Url response: {url}\nResponse: {request.downloadHandler.text}");
			onResponse(request.downloadHandler.text);
		}
	}
}