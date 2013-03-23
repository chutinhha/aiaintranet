
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace AIA.Intranet.Common.Helpers
{
	/// <summary>
	/// Converts HTML to plain text.
	/// </summary>
	public class HtmlToTextConverter
	{
		// Static data tables
		protected static Dictionary<string, string> tags;
		protected static HashSet<string> ignoreTags;

		// Instance variables
		protected TextBuilder builder;
		protected string html;
		protected int pos;

		// Static constructor (one time only)
        static HtmlToTextConverter()
		{
			tags = new Dictionary<string, string>();
			tags.Add("address", "\n");
			tags.Add("blockquote", "\n");
			tags.Add("div", "\n");
			tags.Add("dl", "\n");
			tags.Add("fieldset", "\n");
			tags.Add("form", "\n");
			tags.Add("h1", "\n");
			tags.Add("/h1", "\n");
			tags.Add("h2", "\n");
			tags.Add("/h2", "\n");
			tags.Add("h3", "\n");
			tags.Add("/h3", "\n");
			tags.Add("h4", "\n");
			tags.Add("/h4", "\n");
			tags.Add("h5", "\n");
			tags.Add("/h5", "\n");
			tags.Add("h6", "\n");
			tags.Add("/h6", "\n");
			tags.Add("p", "\n");
			tags.Add("/p", "\n");
			tags.Add("table", "\n");
			tags.Add("/table", "\n");
			tags.Add("ul", "\n");
			tags.Add("/ul", "\n");
			tags.Add("ol", "\n");
			tags.Add("/ol", "\n");
			tags.Add("/li", "\n");
			tags.Add("br", "\n");
			tags.Add("/td", "\t");
			tags.Add("/tr", "\n");
			tags.Add("/pre", "\n");

			ignoreTags = new HashSet<string>();
			ignoreTags.Add("script");
			ignoreTags.Add("noscript");
			ignoreTags.Add("style");
			ignoreTags.Add("object");
		}

		/// <summary>
		/// Converts the given HTML to plain text and returns the result.
		/// </summary>
		/// <param name="html">HTML to be converted</param>
		/// <returns>Resulting plain text</returns>
		public string Convert(string sourceHtml)
		{
			// Initialize state variables
			builder = new TextBuilder();
            html = sourceHtml;
			pos = 0;

			// Process input
			while (!EndOfText)
			{
				if (Peek() == '<')
				{
					// HTML tag
					bool selfClosing;
					string tag = ParseTag(out selfClosing);

					// Handle special tag cases
					if (tag == "body")
					{
						// Discard content before <body>
						builder.Clear();
					}
					else if (tag == "/body")
					{
						// Discard content after </body>
						pos = html.Length;
					}
					else if (tag == "pre")
					{
						// Enter preformatted mode
						builder.Preformatted = true;
						EatWhitespaceToNextLine();
					}
					else if (tag == "/pre")
					{
						// Exit preformatted mode
						builder.Preformatted = false;
					}

					string value;
					if (tags.TryGetValue(tag, out value))
						builder.Write(value);

					if (ignoreTags.Contains(tag))
						EatInnerContent(tag);
				}
				else if (Char.IsWhiteSpace(Peek()))
				{
					// Whitespace (treat all as space)
					builder.Write(builder.Preformatted ? Peek() : ' ');
					MoveAhead();
				}
				else
				{
					// Other text
					builder.Write(Peek());
					MoveAhead();
				}
			}
			// Return result
			return HttpUtility.HtmlDecode(builder.ToString());
		}

		// Eats all characters that are part of the current tag
		// and returns information about that tag
		protected string ParseTag(out bool selfClosing)
		{
			string tag = String.Empty;
			selfClosing = false;

			if (Peek() == '<')
			{
				MoveAhead();

				// Parse tag name
				EatWhitespace();
				int start = pos;
				if (Peek() == '/')
					MoveAhead();
				while (!EndOfText && !Char.IsWhiteSpace(Peek()) &&
					Peek() != '/' && Peek() != '>')
					MoveAhead();
				tag = html.Substring(start, pos - start).ToLower();

				// Parse rest of tag
				while (!EndOfText && Peek() != '>')
				{
					if (Peek() == '"' || Peek() == '\'')
						EatQuotedValue();
					else
					{
						if (Peek() == '/')
							selfClosing = true;
						MoveAhead();
					}
				}
				MoveAhead();
			}
			return tag;
		}

		// Consumes inner content from the current tag
		protected void EatInnerContent(string tag)
		{
			string endTag = "/" + tag;

			while (!EndOfText)
			{
				if (Peek() == '<')
				{
					// Consume a tag
					bool selfClosing;
					if (ParseTag(out selfClosing) == endTag)
						return;
					// Use recursion to consume nested tags
					if (!selfClosing && !tag.StartsWith("/"))
						EatInnerContent(tag);
				}
				else MoveAhead();
			}
		}

		// Returns true if the current position is at the end of
		// the string
		protected bool EndOfText
		{
			get { return (pos >= html.Length); }
		}

		// Safely returns the character at the current position
		protected char Peek()
		{
			return (pos < html.Length) ? html[pos] : (char)0;
		}

		// Safely advances to current position to the next character
		protected void MoveAhead()
		{
			pos = Math.Min(pos + 1, html.Length);
		}

		// Moves the current position to the next non-whitespace
		// character.
		protected void EatWhitespace()
		{
			while (Char.IsWhiteSpace(Peek()))
				MoveAhead();
		}

		// Moves the current position to the next non-whitespace
		// character or the start of the next line, whichever
		// comes first
		protected void EatWhitespaceToNextLine()
		{
			while (Char.IsWhiteSpace(Peek()))
			{
				char c = Peek();
				MoveAhead();
				if (c == '\n')
					break;
			}
		}

		// Moves the current position past a quoted value
		protected void EatQuotedValue()
		{
			char c = Peek();
			if (c == '"' || c == '\'')
			{
				// Opening quote
				MoveAhead();
				// Find end of value
				int start = pos;
				pos = html.IndexOfAny(new char[] { c, '\r', '\n' }, pos);
				if (pos < 0)
					pos = html.Length;
				else
					MoveAhead();	// Closing quote
			}
		}

		/// <summary>
		/// A StringBuilder class that helps eliminate excess whitespace.
		/// </summary>
		protected class TextBuilder
		{
			private StringBuilder text;
			private StringBuilder currLine;
			private int emptyLines;
			private bool preformatted;

			// Construction
			public TextBuilder()
			{
				text = new StringBuilder();
				currLine = new StringBuilder();
				emptyLines = 0;
				preformatted = false;
			}

			/// <summary>
			/// Normally, extra whitespace characters are discarded.
			/// If this property is set to true, they are passed
			/// through unchanged.
			/// </summary>
			public bool Preformatted
			{
				get
				{
					return preformatted;
				}
				set
				{
					if (value)
					{
						// Clear line buffer if changing to
						// preformatted mode
						if (currLine.Length > 0)
							FlushCurrLine();
						emptyLines = 0;
					}
					preformatted = value;
				}
			}

			/// <summary>
			/// Clears all current text.
			/// </summary>
			public void Clear()
			{
				text.Length = 0;
				currLine.Length = 0;
				emptyLines = 0;
			}

			/// <summary>
			/// Writes the given string to the output buffer.
			/// </summary>
			/// <param name="s"></param>
			public void Write(string s)
			{
				foreach (char c in s)
					Write(c);
			}

			/// <summary>
			/// Writes the given character to the output buffer.
			/// </summary>
			/// <param name="c">Character to write</param>
			public void Write(char c)
			{
				if (preformatted)
				{
					// Write preformatted character
					text.Append(c);
				}
				else
				{
					if (c == '\r')
					{
						// Ignore carriage returns. We'll process
						// '\n' if it comes next
					}
					else if (c == '\n')
					{
						// Flush current line
						FlushCurrLine();
					}
					else if (Char.IsWhiteSpace(c))
					{
						// Write single space character
						int len = currLine.Length;
						if (len == 0 || !Char.IsWhiteSpace(currLine[len - 1]))
							currLine.Append(' ');
					}
					else
					{
						// Add character to current line
						currLine.Append(c);
					}
				}
			}

			// Appends the current line to output buffer
			protected void FlushCurrLine()
			{
				// Get current line
				string line = currLine.ToString().Trim();

				// Determine if line contains non-space characters
				string tmp = line.Replace("&nbsp;", String.Empty);
				if (tmp.Length == 0)
				{
					// An empty line
					emptyLines++;
					if (emptyLines < 2 && text.Length > 0)
						text.AppendLine(line);
				}
				else
				{
					// A non-empty line
					emptyLines = 0;
					text.AppendLine(line);
				}

				// Reset current line
				currLine.Length = 0;
			}

			/// <summary>
			/// Returns the current output as a string.
			/// </summary>
			public override string ToString()
			{
				if (currLine.Length > 0)
					FlushCurrLine();
				return text.ToString();
			}
		}
	}
}
