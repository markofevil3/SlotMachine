using UnityEngine;
using System.Collections;

using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class ServerRequest {
	public enum Type {
		PUBLIC_MESSAGE,
		EXTENSION
	};
	
	public Type type;
	public string callback;
	public string commandId;
	public GameObject handler;
	public ISFSObject requestData;
	
	public ServerRequest(Type type, string commandId, ISFSObject requestData, GameObject handler, string callback) {
		this.type = type;
		this.commandId = commandId;
		this.requestData = requestData;
		this.handler = handler;
		this.callback = callback;
	}
	
	public string ToString() {
		return type + " " + commandId + " " + requestData + " " + handler + " " + callback;
	}
}