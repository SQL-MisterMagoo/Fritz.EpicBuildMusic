﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fritz.EpicBuildMusic
{

	public class MusicPlayer : IDisposable
	{

		private string _Command;
		private bool _IsOpen;


		[DllImport("winmm.dll")]
		private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

		#region Singleton

		private static readonly MusicPlayer _Instance = new MusicPlayer();

		private MusicPlayer() {
		}

		~MusicPlayer()
		{
			Dispose(false);
		}

		public static MusicPlayer Instance => _Instance;

		#endregion

		public string Filename { get; set; } = @"C:\dev\Fritz.EpicBuildMusic\Fritz.EpicBuildMusic\EpicSax.mp3";

		public void BeginPlaying()
		{
			Trace.WriteLine("Beginning to play music");
			if (!_IsOpen) Open();
			Play();
		}

		public void StopPlaying()
		{
			Trace.WriteLine("Stopping music");
			Close();
		}

		private void Close()
		{
			_Command = "close MediaFile";
			mciSendString(_Command, null, 0, IntPtr.Zero);
			_IsOpen = false;
		}

		private void Open()
		{
			_Command = "open \"" + Filename + "\" type mpegvideo alias MediaFile";
			mciSendString(_Command, null, 0, IntPtr.Zero);
			_IsOpen = true;
		}

		private void Pause()
		{
			_Command = "pause MediaFile";
			mciSendString(_Command, null, 0, IntPtr.Zero);
		}

		private void Play()
		{
			if (_IsOpen)
			{
				_Command = "play MediaFile REPEAT";
				mciSendString(_Command, null, 0, IntPtr.Zero);
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool isDisposing)
		{

			if (isDisposing) GC.SuppressFinalize(this);

			Close();

		}

	}

}
