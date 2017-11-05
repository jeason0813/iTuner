//************************************************************************************************
// Copyright © 2017 Steven M. Cohn. All Rights Reserved.
//************************************************************************************************


namespace iTuner
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Runtime.InteropServices;
	using System.Windows;


	public enum ScreenEdge
	{
		Undefined = -1,
		Left = Interop.ABE_LEFT,
		Top = Interop.ABE_TOP,
		Right = Interop.ABE_RIGHT,
		Bottom = Interop.ABE_BOTTOM
	}


	//********************************************************************************************
	// Interop
	//********************************************************************************************

	[SuppressMessage ("ReSharper", "InconsistentNaming")]
	internal static class Interop
	{

		public const int ABE_BOTTOM = 3;
		public const int ABE_LEFT = 0;
		public const int ABE_RIGHT = 2;
		public const int ABE_TOP = 1;

		public const int ABM_GETTASKBARPOS = 0x00000005;


		[StructLayout (LayoutKind.Sequential)]
		public struct APPBARDATA
		{
			public uint cbSize;
			public IntPtr hWnd;
			public uint uCallbackMessage;
			public uint uEdge;
			public RECT rc;
			public int lParam;
		}

		[StructLayout (LayoutKind.Sequential)]
		public struct GUID
		{
			public uint Data1;
			public ushort Data2;
			public ushort Data3;

			[MarshalAs (UnmanagedType.ByValArray, SizeConst = 8)] public byte[] Data4;
		}


		[StructLayout (LayoutKind.Sequential)]
		public struct NOTIFYICONIDENTIFIER
		{
			public uint cbSize;
			public IntPtr hWnd;
			public uint uID;
			public GUID guidItem; // System.Guid can be used.
		}

		[StructLayout (LayoutKind.Sequential)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;

			// convert to a WPF System.Windows.Rec
			public static implicit operator Rect (RECT rect)
			{
				if ((rect.right - rect.left < 0) || (rect.bottom - rect.top < 0))
					return Rect.Empty;

				return new Rect(
					rect.left,
					rect.top,
					rect.right - rect.left,
					rect.bottom - rect.top);
			}
		}


		[DllImport ("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr FindWindow (
			string strClassName,
			string strWindowName);


		[DllImport ("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowEx (
			IntPtr parentHandle,
			IntPtr childAfter,
			string className,
			IntPtr windowTitle);


		[DllImport ("user32.dll")]
		[return: MarshalAs (UnmanagedType.Bool)]
		public static extern bool GetWindowRect (
			IntPtr hWnd,
			out RECT lpRect);

		[DllImport ("shell32.dll")]
		public static extern uint SHAppBarMessage (
			UInt32 dwMessage,
			ref APPBARDATA data);


		[DllImport ("Shell32.dll", SetLastError = true)]
		public static extern int Shell_NotifyIconGetRect (
			[In] ref NOTIFYICONIDENTIFIER identifier,
			out RECT iconLocation);
	}
}