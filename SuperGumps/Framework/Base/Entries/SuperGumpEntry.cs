#region Header
//   Vorspire    _,-'/-'/  SuperGumpEntry.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;

using Server;
using Server.Gumps;
using Server.Network;
#endregion

namespace VitaNex.SuperGumps
{
	public interface IGumpEntryPoint
	{
		int X { get; set; }
		int Y { get; set; }
	}

	public interface IGumpEntrySize
	{
		int Width { get; set; }
		int Height { get; set; }
	}

	public interface IGumpEntryVector : IGumpEntryPoint, IGumpEntrySize
	{ }

	public abstract class SuperGumpEntry : GumpEntry, IDisposable
	{
		public virtual bool IgnoreModalOffset { get { return false; } }

		public virtual Mobile User
		{
			get
			{
				if (Parent is SuperGump)
				{
					return ((SuperGump)Parent).User;
				}

				return null;
			}
		}

		public virtual NetState UserState
		{
			get
			{
				var user = User;

				if (user != null)
				{
					return user.NetState;
				}

				return null;
			}
		}

		protected int FixHue(int hue)
		{
			return FixHue(hue, false);
		}

		protected int FixHue(int hue, bool item)
		{
			hue = Math.Max(-1, hue & 0x7FFF);

			if (hue <= 0)
			{
				return 0;
			}

			hue = Math.Min(3000, hue) - 1;

			if (item || UserState.IsEnhanced())
			{
				++hue;
			}

			return hue;
		}

		protected void Delta<T>(ref T var, T val)
		{
			if (ReferenceEquals(var, val) || Equals(var, val))
			{
				return;
			}

			var old = var;

			var = val;

			OnInvalidate(old, val);
			OnInvalidate();
		}

		protected virtual void OnInvalidate<T>(T old, T val)
		{ }

		protected virtual void OnInvalidate()
		{
			if (Parent != null)
			{
				Parent.Invalidate();
			}
		}

		public virtual void Dispose()
		{
			Parent = null;
		}
	}
}