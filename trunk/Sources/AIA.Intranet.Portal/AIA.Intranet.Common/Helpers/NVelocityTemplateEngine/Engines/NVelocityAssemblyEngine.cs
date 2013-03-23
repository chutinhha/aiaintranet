using System.Collections;
using System.IO;
using NVelocity;
using NVelocity.Exception;
using NVelocity.Runtime;
using AIA.Intranet.Common.NVelocityTemplateEngine.BaseClasses;
using AIA.Intranet.Common.NVelocityTemplateEngine.Interfaces;

namespace AIA.Intranet.Common.NVelocityTemplateEngine.Engines
{
	/// <summary>
	/// Summary description for NVelocityAssemblyEngine.
	/// </summary>
	public sealed class NVelocityAssemblyEngine : NVelocityEngineBase, INVelocityEngine
	{
		private string assemblyName;

        /// <summary>ConsoleApplication5
		/// Initializes a new instance of the <see cref="NVelocityAssemblyEngine"/> class.
		/// </summary>
		/// <param name="assemblyName">Name of the assembly.</param>
		/// <param name="cacheTamplate">if set to <c>true</c> [cache tamplate].</param>
		internal NVelocityAssemblyEngine(string assemblyName, bool cacheTamplate) : base(cacheTamplate)
		{	
			this.assemblyName = assemblyName;
	
			this.SetProperty(RuntimeConstants.RESOURCE_LOADER, "assembly");
			this.SetProperty("assembly.resource.loader.class", "NVelocity.Runtime.Resource.Loader.AssemblyResourceLoader;NVelocity");
			this.SetProperty("assembly.resource.loader.assembly", assemblyName);
			this.Init();	
		}

		/// <summary>
		/// Processes the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="templateName">Name of the template.</param>
		/// <returns></returns>
		public string Process(IDictionary context, string templateName)
		{
			StringWriter writer = new StringWriter();

			try
			{	
				Template template = this.GetTemplate(this.assemblyName + "." + templateName);
				template.Merge(CreateContext(context), writer);
			}
			catch (ResourceNotFoundException rnf)
			{	
				return rnf.Message;
			} 
			catch (ParseErrorException pe)
			{
				return pe.Message;
			}
			
			return writer.ToString();
		}

		/// <summary>
		/// Processes the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="writer">The writer.</param>
		/// <param name="templateName">Name of the template.</param>
		public void Process(IDictionary context, TextWriter writer, string templateName)
		{
			try
			{	
				Template template = this.GetTemplate(this.assemblyName + "." + templateName);
				template.Merge(CreateContext(context), writer);
			}
			catch (ResourceNotFoundException rnf)
			{	
				writer.Write(rnf.Message);
			} 
			catch (ParseErrorException pe)
			{
				writer.Write(pe.Message);
			}		
		}
	}
}
