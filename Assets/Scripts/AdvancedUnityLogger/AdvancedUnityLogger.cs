using UnityEngine;

public class AdvancedUnityLogger {
	
	private static string _assemblyName = "Assembly-CSharp-Editor";
	private static string _className = "AdvancedLogWindow";
	private static string _methodName = "Log";
	
	//private static Assembly _asm;
	//private static Type _loggerType;
	//private static object _activator;
	//private static MethodInfo _logMethod;
	
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
			//Debug.Log("sender is string");
			return "###"+(sender as string)+"###";
		}
		if (sender is Component)
		{
			//Debug.Log((sender as Component).gameObject.name);
			return "###"+(sender as Component).gameObject.name+"("+sender.GetType().ToString()+")###";
		}
		return "######";
	}
}
