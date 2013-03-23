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
	/// Summary description for NVelocityFileEngine.
	/// </summary>
	public sealed class NVelocityFileEngine : NVelocityEngineBase, INVelocityEngine
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NVelocityFileEngine"/> class.
		/// </summary>
		/// <param name="templateDirectory">The template directory.</param>
		/// <param name="cacheTemplate">if set to <c>true</c> [cache template].</param>
		internal NVelocityFileEngine(string templateDirectory, bool cacheTemplate) : base(cacheTemplate)
		{
			this.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
			this.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, templateDirectory);
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
				Template template = this.GetTemplate(templateName);
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
				Template template = this.GetTemplate(templateName);
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
