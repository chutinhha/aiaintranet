using System.Collections;
using System.IO;

namespace AIA.Intranet.Common.NVelocityTemplateEngine.Interfaces
{
	/// <summary>
	/// Summary description for INVelocityEngine.
	/// </summary>
	public interface INVelocityEngine
	{
		/// <summary>
		/// Processes the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="template">Name of the template.</param>
		/// <returns></returns>
		string Process(IDictionary context, string template);

		/// <summary>
		/// Processes the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="writer">The writer.</param>
		/// <param name="template">Name of the template.</param>
        void Process(IDictionary context, TextWriter writer, string template);
	}
}
