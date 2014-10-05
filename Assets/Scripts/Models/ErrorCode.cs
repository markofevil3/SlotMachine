using UnityEngine;
using System.Collections;

public class ErrorCode {
  
  public enum USER {
    NULL,
    USER_EXIST,
    USER_NOT_EXIST,
    PASSWORD_NOT_MATCH,
    MAX_FRIENDS,
    CANT_FIND_USER,
    ALREADY_FRIEND
  }
  
	public enum TLMB {
		NULL,
		WRONG_TURN,
		CARDS_NOT_EXIST,
		CANNOT_DEFEAT,
		CANNOT_DROP,
		INVALID_GAMECONFIG,
		CANNOT_CREATE_ROOM,
		UNKNOWN = 100
	}
}