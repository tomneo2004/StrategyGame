using UnityEngine;
using System.Collections;
using System;

public class TimeConverter
{
	const int _secondToDay = 86400;
	const int _secondToHour = 3600;
	const int _secondToMinute = 60;

	public static int GetSeconds(int totalSeconds)
	{
		//return (((totalSeconds % _secondToDay) % _secondToHour) % _secondToMinute);
		return ((totalSeconds % _secondToHour) % _secondToMinute);
	}

	public static int GetMinutes(int totalSeconds)
	{
		//return ((totalSeconds % _secondToDay) % _secondToHour) / _secondToMinute;
		return (totalSeconds % _secondToHour) / _secondToMinute;
	}

	public static int GetHours(int totalSeconds)
	{
		//return (totalSeconds % _secondToDay) / _secondToHour;
		return totalSeconds / _secondToHour;
	}

	/*
	public static int GetDays(int totalSeconds)
	{
		return totalSeconds / _secondToDay;
	}
	*/

	public static DateTime TimeWithoutMillisecond(DateTime inTime)
	{
		return new DateTime (inTime.Year, inTime.Month, inTime.Day, inTime.Hour, inTime.Minute, inTime.Second);
	}

	/// <summary>
	/// Seconds to time string.
	/// Give time in second and return time hour:Minute:Second in string
	/// </summary>
	/// <returns>The to time string.</returns>
	/// <param name="totalSecond">Total second.</param>
	public static string SecondToTimeString(int totalSecond)
	{
		string displayStr = "";
		
		int hours = TimeConverter.GetHours(totalSecond);
		int minutes = TimeConverter.GetMinutes(totalSecond);
		int seconds = TimeConverter.GetSeconds(totalSecond);
		
		
		if(hours > 0)
		{
			displayStr += " "+hours+"h";
		}
		
		if(minutes > 0)
		{
			displayStr += " "+minutes+"m";
		}
		
		if((seconds >= 0) && (hours <= 0) && (minutes <= 0))
		{
			displayStr += " "+seconds+"s";
		}

		return displayStr;
	}
}
