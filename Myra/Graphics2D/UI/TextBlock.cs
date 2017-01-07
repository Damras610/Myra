using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Edit;
using Myra.Graphics2D.Text;
using Myra.Graphics2D.UI.Styles;
using Newtonsoft.Json;

namespace Myra.Graphics2D.UI
{
	public class TextBlock : Widget
	{
		private readonly FormattedText _formattedText = new FormattedText();
		private bool _wrap = true;

		public int VerticalSpacing
		{
			get { return _formattedText.VerticalSpacing; }
			set
			{
				_formattedText.VerticalSpacing = value;
				FireMeasureChanged();
			}
		}

		public string Text
		{
			get { return _formattedText.Text; }
			set
			{
				_formattedText.Text = value;
				FireMeasureChanged();
			}
		}

		[HiddenInEditor]
		[JsonIgnore]
		public BitmapFont Font
		{
			get { return _formattedText.Font; }
			set
			{
				_formattedText.Font = value;
				FireMeasureChanged();
			}
		}

		public bool Wrap
		{
			get { return _wrap; }

			set
			{
				if (value == _wrap)
				{
					return;
				}

				_wrap = value;
				FireMeasureChanged();
			}
		}

		public Color TextColor { get; set; }

		public Color DisabledTextColor { get; set; }

		public TextBlock(TextBlockStyle style)
		{
			if (style != null)
			{
				ApplyTextBlockStyle(style);
			}
		}

		public TextBlock() : this(DefaultAssets.UIStylesheet.TextBlockStyle)
		{
		}

		public override void InternalRender(SpriteBatch batch, Rectangle bounds)
		{
			if (_formattedText.Font == null)
			{
				return;
			}

			_formattedText.Draw(batch, bounds, Enabled?TextColor:DisabledTextColor);
		}

		protected override Point InternalMeasure(Point availableSize)
		{
			if (Font == null)
			{
				return Point.Zero;
			}

			var width = availableSize.X;
			if (WidthHint != null && WidthHint.Value < width)
			{
				width = WidthHint.Value;
			}

			var result = Point.Zero;
			if (Font != null)
			{
				var formattedText = _formattedText.Clone();
				formattedText.Width = _wrap ? width : default(int?);

				result = formattedText.Size;
			}

			if (result.Y < Font.LineHeight)
			{
				result.Y = Font.LineHeight;
			}

			return result;
		}

		public override void Arrange()
		{
			base.Arrange();

			_formattedText.Width = _wrap ? LayoutBounds.Width : default(int?);
		}

		public void ApplyTextBlockStyle(TextBlockStyle style)
		{
			ApplyWidgetStyle(style);

			TextColor = style.TextColor;
			DisabledTextColor = style.DisabledTextColor;
			Font = style.Font;
		}
	}
}