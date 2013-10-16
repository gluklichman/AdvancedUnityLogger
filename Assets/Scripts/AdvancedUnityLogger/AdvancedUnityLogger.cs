using UnityEngine;

public class AdvancedUnityLogger {
	
	public static void Log(object message, object sender)
	{
		//generate sender label
		string senderLabel = GetSenderLabel(sender);
		//prepare message
		string messageWithSender = senderLabel+message;
		//send message
		Debug.Log(messageWithSender);
	}
	
	public static string GetSenderLabel(object sender)
	{
		if (sender is string)
		{
			return "###"+(sender as string)+"###";
		}
		if (sender is Component)
		{
			return "###"+(sender as Component).gameObject.name+"("+sender.GetType().ToString()+")###";
		}
		return "######";
	}
}
