using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notpad.Client
{
	public class NonSelectingTextbox : TextBox
	{
		private bool selectionSet;
		protected override void OnGotFocus(EventArgs e)
		{
			bool needToDeselect = false;

			if (!selectionSet)
			{
				selectionSet = true;

				if ((SelectionLength == 0) && MouseButtons == MouseButtons.None)
					needToDeselect = true;
			}

			base.OnGotFocus(e);

			if (needToDeselect)
			{
				SelectionStart = Text.Length;
				DeselectAll();
			}
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				selectionSet = false;
			}
		}
	}
}
