using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class ServerRequestQueue {
	private static List<ServerRequest> requests;
	
	public static void Init() {
		requests = new List<ServerRequest>();
	}
	
	public static void Queue(ServerRequest request) {
		requests.Add(request);
	}
	
	public static void QueueOnTop(ServerRequest request) {
		requests.Insert(0, request);
	}
	
	public static ServerRequest Dequeue() {
		ServerRequest request = null;

		if (requests.Count > 0) {
			request = requests[0];
			requests.RemoveAt(0);
		}

		return request;
	}
	
	public static void Clear() {
		requests.Clear();
	}
}