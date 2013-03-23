using System;
using System.IO;
using System.Text;

namespace AIA.Intranet.Model.Workflow
{
    public class InfoPathAttachment
    {
        //  Private byte array to hold the decoded attachment.
        private readonly byte[] decodedFile;
        
        //  Private string to hold the attachment name.
        private readonly string fileName;

        /// <summary>
        /// Constructor for the InfoPathAttachmentDecoder Class
        /// </summary>
        /// <param name="base64EncodedString">The attachment represented by a string</param>
        public InfoPathAttachment(string base64EncodedString)
        {
            //  Use unicode encoding.
            Encoding encoding = Encoding.Unicode;

            //  The byte array containing the data.
            byte[] data = Convert.FromBase64String(base64EncodedString);

            //  Use a memory stream to access the data.
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                //  Create a binary reader from the stream.
                BinaryReader theReader = new BinaryReader(memoryStream);

                //  Create a byte array to hold the header data.
                byte[] headerData = theReader.ReadBytes(16);

                //  Find the file size before finding the file name.
                int fileSize = (int)theReader.ReadUInt32();

                //  Get the file name.
                int attachmentNameLength = (int)theReader.ReadUInt32() * 2;

                byte[] fileNameBytes = theReader.ReadBytes(attachmentNameLength);

                this.fileName = encoding.GetString(fileNameBytes, 0, attachmentNameLength - 2);

                //  Get the decoded attachment.            
                this.decodedFile = theReader.ReadBytes(fileSize);
            }
        }

        /// <summary>
        /// Constructor for the InfoPathAttachmentDecoder Class
        /// </summary>
        /// <param name="base64EncodedBytes">The attachment represented by a byte array</param>
        public InfoPathAttachment(byte[] base64EncodedBytes)
            : this(Convert.ToBase64String(base64EncodedBytes))
        {
        }

        /// <summary>
        /// The name of the file within the InfoPath attachment.
        /// </summary>
        public string Filename
        {
            get { return this.fileName; }
        }

        /// <summary>
        /// The description of the file within the InfoPath attachment.
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// The decoded file within the InfoPath attachment.
        /// </summary>
        public byte[] DecodedFile
        {
            get { return this.decodedFile; }
        }

        /// <summary>
        /// Static method that gets the file from the attachment.
        /// </summary>
        /// <param name="base64EncodedString">The attachment represented by a string</param>
        /// <returns>Returns a byte array of the file in the attachment.</returns>
        public static byte[] DecodeInfoPathAttachment(string base64EncodedString)
        {
            //  Create an instance of the InfoPathAttachmentDecoder
            InfoPathAttachment infoPathAttachmentDecoder = new InfoPathAttachment(base64EncodedString);

            //  Return the decoded file.
            return infoPathAttachmentDecoder.DecodedFile;
        }

        /// <summary>
        /// Static method that gets the file from the attachment.
        /// </summary>
        /// <param name="base64EncodedBytes">The attachment represented by a byte array</param>
        /// <returns>Returns a byte array of the file in the attachment.</returns>
        public static byte[] DecodeInfoPathAttachment(byte[] base64EncodedBytes)
        {
            //  Create an instance of the InfoPathAttachmentDecoder
            InfoPathAttachment infoPathAttachmentDecoder = new InfoPathAttachment(base64EncodedBytes);

            //  Return the decoded file.
            return infoPathAttachmentDecoder.DecodedFile;
        }
    }
}
