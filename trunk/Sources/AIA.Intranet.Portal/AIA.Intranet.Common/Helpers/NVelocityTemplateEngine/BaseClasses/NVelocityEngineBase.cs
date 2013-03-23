using System.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;

namespace AIA.Intranet.Common.NVelocityTemplateEngine.BaseClasses
{
	/// <summary>
	/// Summary description for NVelocityEngineBase.
	/// </summary>
	public abstract class NVelocityEngineBase : VelocityEngine
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NVelocityEngineBase"/> class.
		/// </summary>
		/// <param name="cacheTemplate">if set to <c>true</c> [cache template].</param>
		protected NVelocityEngineBase(bool cacheTemplate) : base()
		{
			this.SetProperty("assembly.resource.loader.cache", cacheTemplate.ToString().ToLower() );
		}

		/// <summary>
		/// Creates a VelocityContext from an IDictionary object.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		protected static IContext CreateContext(IDictionary context)
		{
			return new VelocityContext(new Hashtable(context));
		}
	}
}
