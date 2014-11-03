using UnityEngine;
using System.Collections;

public class Command {
  public class USER {
    public const string LOAD_LEADERBOARD = "loadLeaderboard";
    public const string LOAD_USER_INFO = "loadUserInfo";
    public const string ADD_FRIEND = "addFriend";
    public const string CHAT_IN_ROOM = "chatInRoom";
    public const string INVITE_TO_GAME = "inviteToGame";
  }
  
	public class TLMB {
		public const string LOBBY = "lobby";
		public const string CREATE = "create";
		public const string START = "start";
		public const string JOIN = "join";
		public const string KICK = "kick";
		public const string DROP = "drop";
		public const string FOLD = "fold";
		public const string SIT = "sit";
		public const string STANDUP = "standup";
		public const string LEAVE = "leave";
		public const string QUIT = "quit";
		public const string UPDATE = "update";
	}

  public class SLOT_MACHINE {
    public const string SLOT_JOIN_ROOM = "slotJoinRoom";
    public const string SLOT_PLAY = "slotPlay";
    public const string SLOT_LEAVE = "slotLeave";
		public const string SLOT_TYPE_FRUITS = "slot_fruit";
		public const string SLOT_TYPE_HALLOWEEN = "slot_halloween";
		public const string SLOT_TYPE_DRAGON = "slot_dragon";
  }
  
	public static string Create(string gameId, string commandId) {
		return gameId + "." + commandId;
	}
}