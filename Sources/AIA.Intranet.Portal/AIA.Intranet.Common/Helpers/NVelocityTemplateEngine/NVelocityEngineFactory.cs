using AIA.Intranet.Common.NVelocityTemplateEngine.Engines;
using AIA.Intranet.Common.NVelocityTemplateEngine.Interfaces;

namespace AIA.Intranet.Common.NVelocityTemplateEngine
{
	/// <summary>
	/// Summary description for NVelocityEngineFactory.
	/// </summary>
	public class NVelocityEngineFactory
	{
		/// <summary>
		/// Creates a new instance of NVelocityFileEngine class..
		/// </summary>
		/// <param name="templateDirectory">The template directory.</param>
		/// <param name="cacheTemplate">if set to <c>true</c> [cache template].</param>
		/// <returns></returns>
		public static INVelocityEngine CreateNVelocityFileEngine(string templateDirectory, bool cacheTemplate)
		{
			return new NVelocityFileEngine(templateDirectory, cacheTemplate);
		}

		/// <summary>
		/// Creates a new instance of NVelocityAssemblyEngine class.
		/// </summary>
		/// <param name="assemblyName">Name of the assembly.</param>
		/// <param name="cacheTemplate">if set to <c>true</c> [cache template].</param>
		/// <returns></returns>
		public static INVelocityEngine CreateNVelocityAssemblyEngine(string assemblyName, bool cacheTemplate)
		{
			return new NVelocityAssemblyEngine(assemblyName, cacheTemplate);
		}

		/// <summary>
		/// Creates a new instance of NVelocityMemoryEngine class.
		/// </summary>
		/// <param name="cacheTemplate">if set to <c>true</c> [cache template].</param>
		/// <returns></returns>
		public static INVelocityEngine CreateNVelocityMemoryEngine(bool cacheTemplate)
		{
			return new NVelocityMemoryEngine(cacheTemplate);
		}
	}
}
