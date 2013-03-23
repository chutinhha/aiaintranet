using System;
using System.Runtime.Serialization;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Workflow;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
    public class TaskDetail : SPAutoSerializingObject, ISerializable 
    //TODO: May not need ISerializable here.
    {
        #region Constructors

        public TaskDetail()
        {

        }

        protected TaskDetail(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        [Persisted]
        public int TaskId = 0;

        [Persisted]
        public Guid Id = Guid.Empty;
        
        [Persisted]
        public String ContentTypeId = String.Empty;
        
        [Persisted]
        public String Title = String.Empty;
        
        [Persisted]
        public String Body = String.Empty;
        
        [Persisted]
        public String InitialAssignTo = String.Empty;

        [Persisted]
        public String AssignToEmail = String.Empty;

        [Persisted]
        public String Status = String.Empty;

        [Persisted]
        public string StatusWorkflow = string.Empty;

        [Persisted]
        public float PercentComplete = 0.0f;
        
        [Persisted]
        public int Type = 0;

        [Persisted]
        public DateTime DueDate = new DateTime();


        [Persisted]
        public int PreviousTaskId = 0;

        [Persisted]
        public bool InfoPathForm = false;

        [Persisted]
        public SPWorkflowTaskProperties TaskProperties;

        [Persisted]
        public SPWorkflowTaskProperties TaskAfterProperties;

        [Persisted]
        public String CCEmail = String.Empty;
        /// <summary>
        /// Resets Critical Properties if this TaskDetail is to be used for a new task.
        /// WARNING: All prior detail stored in TaskProperties will be lost after this.
        /// </summary>
        public void ResetProperties()
        {
            this.Id = Guid.NewGuid(); // Create a new Id for the second go around.
            this.PercentComplete = 0.0f;
            this.TaskProperties = new SPWorkflowTaskProperties();
            this.TaskAfterProperties = new SPWorkflowTaskProperties();
        }

        public void InitializeTaskProperties()
        {
            // Set the reset of the task properties.
            TaskProperties.Title = this.Title;
            TaskProperties.Description = this.Body;
            TaskProperties.PercentComplete = this.PercentComplete;
            TaskProperties.AssignedTo = this.InitialAssignTo;
            TaskProperties.TaskType = this.Type;
        }

        #region ISerializable Members

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }
}
