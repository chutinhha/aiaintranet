using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model
{
    public class IOfficeColumnId
    {
        #region SiteColumn
        public static Guid DepartmentSiteGroup = new Guid("{3752053D-DCF4-469f-B75E-A3E2D8380BAA}");
        public static Guid PublicOrPrivate = new Guid("{D558E7F2-2B25-4c3c-B6A6-96FABAF8A1AD}");
        public static Guid LastUpdatedByWF = new Guid("{8C3A6A79-901E-44FF-8A00-3D35363C6DEB}");

        public static Guid Attendees = new Guid("{9E13F687-DD1C-44ED-89A5-83BF27A89959}");
        #endregion SiteColumn

        #region Emplyee list
        public class Employees
        {
            public static Guid IAvatar = new Guid("{BB06603F-C5E8-47CB-8A59-9D1B718E81BC}");
            public static Guid EmployeeCode = new Guid("{1962c3cf-78bc-44cc-ab86-d6ccbd6148c1}");
            public static Guid Account = new Guid("{81963DBF-2274-4E3B-B636-68D534F94632}");
            public static Guid EmployeeName = new Guid("{0CD99421-7F16-4AD8-ADE1-7E7A36B26A5E}");
            public static Guid BirthDay = new Guid("{210a1057-44c0-4bf0-a157-69b9f5c6181f}");
            public static Guid MonthOfBirth = new Guid("{22A82C85-1B70-4092-95B3-E33B1034556A}");
            public static Guid PersonalId = new Guid("{191238e5-f7d3-485f-b6cd-b25c304bd8e4}");
            public static Guid CreatedDate = new Guid("{e1ba3443-a559-417a-8833-ae9a6b4028c5}");
            public static Guid LegalAddress = new Guid("{c603e45a-be49-467f-9f93-5cd3e6522df3}");
            public static Guid Education = new Guid("{ca0a4d8e-715c-4dd9-a03b-f839a425ac4c}");
            public static Guid Expertise = new Guid("{1857d2f7-14b8-4120-aecd-42a9f6f2f436}");
            public static Guid ForeignLanguage = new Guid("{7f99ce06-cbc0-417c-8ca2-efd95466f201}");
            public static Guid Position = new Guid("{ef92f6b5-0995-48e8-944d-42c7291ca98a}");
            public static Guid Department = new Guid("{e18ab517-12c0-43fd-aa9f-487d351db5e4}");
            public static Guid PhoneNumber = new Guid("{f7561488-9cff-4d88-a324-14c936ea5bdb}");
            public static Guid MobileNumber = new Guid("{65FCE0FD-061A-4754-A6BF-6EFD1E6E69A8}");
            public static Guid Email = new Guid("{7bd77fdb-0fc6-4c06-9853-fb1b4d311cb8}");
            public static Guid BeginDate = new Guid("{b4a8ac72-a257-4a28-871b-9e6c9213d100}");
            public static Guid EndDate = new Guid("{93787D02-9985-4535-842D-26364923E0D5}");
            public static Guid PlaceOfBirth = new Guid("{e8750d3f-66d8-44b5-a892-f571d7d98972}");
            public static Guid Gender = new Guid("{0975e684-3165-40c4-8ab7-ea88338ec3b5}");
            public static Guid LaborInsuranceCode = new Guid("{A467AFF2-014B-47EA-9F1C-1EB483F730B4}");
            public static Guid SocialInsuranceCode = new Guid("{703EF8E9-F2C4-4446-8B66-C2937A2BF5B8}");
            public static Guid LaborCode = new Guid("{3795B543-6491-41D3-9AE8-CA9C6722AE9A}");
            public static Guid Nick = new Guid("{89EE66BA-D4FB-4241-A563-EBD26F30B02B}");
            public static Guid Status = new Guid("{7658E072-C0E2-4572-8567-717DD5E0F642}");
            //public static Guid DepartmentSiteGroup = new Guid("{3752053D-DCF4-469f-B75E-A3E2D8380BAA}"); //site column
            public static Guid EmployeeLogin = new Guid("{E1D4CB6F-35D0-4777-8954-364983A14F24}");
        }
        #endregion Emplyee list

        #region Emplyee Document list
        public class EmployeeDocument
        {
            public static Guid DocumentType = new Guid("{573D3AD8-1E99-4589-814F-12D76756A412}");
        }
        #endregion Emplyee list

        #region TopEmployee list
        public class TopEmployee
        {
            public static Guid EmployeeName = new Guid("{E842ADF1-5994-45CB-8844-292F7B1D8A7C}");
            public static Guid Position = new Guid("{C0F3FC63-999E-46F4-93BA-7BB5CAD72D58}");
            public static Guid Department = new Guid("{E0431851-86C3-439A-87D3-AA5997E0B2C2}");
            public static Guid PhoneNumber = new Guid("{FD2146FF-61B6-400A-8EB4-CD48FEE4A510}");
            public static Guid BeginDate = new Guid("{C1EA4AC7-2128-4BCB-AB88-18229E58DB77}");
            public static Guid Year = new Guid("{F795402E-2AA8-41e7-8BA2-AC6D530CDE9A}");
            public static Guid MonthQuaterYear = new Guid("{24BD2D42-1696-45A1-BAB9-EA864033D6A5}");
            public static Guid Note = new Guid("{A3641C28-61D8-4AA9-90FC-0DACBC54A4CE}");
        }
        #endregion TopEmployee list

        #region Link list
        public class ILink
        {
            public static Guid Type = new Guid("{F3825244-921C-4f99-8EA8-226817E7A8DD}");
            public static Guid LinkUrl = new Guid("{4527FEC8-78F4-43e6-B821-045394F45388}");
            public static Guid Category = new Guid("{D02C7013-1AC0-4444-B200-4B07D7F7CEDF}");
        }
        #endregion Link list

        #region Project list
        public class Project
        {
            public static Guid ProjectDescription = new Guid("{CF4CD261-2AA5-493c-8A98-87B724EC7C33}");
            public static Guid Department = new Guid("{1875DFC2-70A7-4c55-82F1-AC505A326E4D}");
            public static Guid ProgramManager = new Guid("{65B52053-425C-44ad-94DD-0E1AE4CB577B}");
            public static Guid ProjectManager = new Guid("{5C42CED6-F00E-4a7a-931D-A2E47D2E14A9}");
            public static Guid ProjectClerk = new Guid("{5B3AEA34-0575-4da6-A98D-A71104AF5F4B}");
            public static Guid ProjectMember = new Guid("{EE18EFC4-5E7D-4226-AA6F-A9422FB04C01}");
            public static Guid ProjectPriority = new Guid("{BCA234A0-ABEE-4d1b-ABFD-5DEA83A4CF21}");
            public static Guid ProjectStart = new Guid("{B046FA89-BE17-4495-9115-7C564EEE8EC3}");
            public static Guid ProjectEnd = new Guid("{9557E0BA-387D-4365-9343-705C009E4563}");
            public static Guid ProjectActualStart = new Guid("{B21266AD-3342-45cc-AED1-15B6E04AEF0E}");
            public static Guid ProjectActualEnd = new Guid("{A4044404-0C9D-4b2b-A341-D0DB554FD0AB}");
            public static Guid ProjectExpenses = new Guid("{9A6417C5-EADC-482f-882F-F0B7C8755F0D}");
            public static Guid ProjectDerivedExpenses = new Guid("{35CF8F53-AA37-4261-8630-76F7FE71B2C5}");
            public static Guid ProjectStatus = new Guid("{E580B056-908C-4c13-9B5D-69DFDF6FEBFB}");
            public static Guid ProjectProgress = new Guid("{CF0B59F0-BC3C-4f11-8E8D-7E90B3B6F3B9}");
            public static Guid ProjectNote = new Guid("{D2AA4333-AF8D-4463-B4E7-305D03F1A52C}");
        }
        #endregion Project list

        #region Department
        public class Department
        {
            public static Guid Description = new Guid("{F1ACD7BB-54B9-46d5-847C-2549A4C87482}");
            public static Guid DepartmentHead = new Guid("{ed4f691a-9faa-4191-9b86-4fd0af5e1de0}");
            public static Guid DeputyDepartmentHead = new Guid("{4f5b954a-0cca-446b-b4a9-80145e5e0750}");
            public static Guid DepartmentAdmin = new Guid("{56390909-8425-4cbd-8755-2420bdb1c245}");
            public static Guid IsActive = new Guid("{5673dddd-6658-4d53-94de-1bc4beacea59}");
            public static Guid PhoneNumber = new Guid("{26995278-be60-4942-8434-2928cd81d380}");
            public static Guid Link = new Guid("{379847fc-ac82-4719-890f-aa2dc49ffcd8}");

            public static Guid PublicOrPrivate = new Guid("{D558E7F2-2B25-4c3c-B6A6-96FABAF8A1AD}");
        }
        #endregion Department

        public class Forum
        {
            public static Guid ForumCategoryPicture = new Guid("{0416ce16-cd78-4b26-abfd-688cce8d6b4f}");
            public static Guid TopicLink = new Guid("{5cb84a1a-d490-49e7-b3eb-0d5fe6771367}");
            public static Guid TopicListId = new Guid("{b99bf5dc-273c-40e3-b95f-cd97ed6eafe9}");
            public static Guid WritePermission = new Guid("{7d8ea451-b7b4-4749-911b-d1bad794114d}");
            public static Guid ParentCat = new Guid("{b0a87fd9-2c2f-49df-97c9-aecca187bd8c}");
            public static Guid ReadPermission = new Guid("{b721d583-9bac-45c4-a4b1-ae1de73458e0}");
            public static Guid TopicCounts = new Guid("{07CFCA4B-4DE4-4CDA-A7FC-9A4B420189CE}");
            public static Guid ReplyCounts = new Guid("{52129556-1a21-4fc5-8572-1ce22e885441}");
            public static Guid ViewCounts = new Guid("{ee645e5c-35f7-4401-9d96-a36f1faa91b7}");
            public static Guid ForumOrder = new Guid("{714E9606-93FF-460E-B85F-791DF8995CF7}");
            public static Guid LastestReply = new Guid("{AE56CE38-6BC4-4370-AE64-D1078FEB9C69}");
            public static Guid ReplyTo = new Guid("{fc0c2fb8-4e63-400e-83c9-16c344963a21}");
            public static Guid Locked = new Guid("{266A7189-7D84-4AA7-9C74-C3EFC0B64C30}");
            public static Guid IsHot = new Guid("{1cdb2077-1199-40b0-b60e-e3710168a842}");
            public static Guid Pinned = new Guid("{82da536b-7c6d-479c-ba05-612d64094b1f}");
            public static Guid PublicUrl = new Guid("{3b408c6c-ee1d-45cc-90b8-8953d375c56f}");
            public static Guid Tags = new Guid("{b24f67e9-42bc-48ef-9ae1-c5199c1e8093}");
            public static Guid ThreadID = new Guid("{052c643c-62c6-4dfd-be94-995199ca2282}");
        }

        #region Task List
        public class Task
        {
            public static Guid TaskManager = new Guid("{0808D172-DEB5-44a2-9FD3-07F934B471D4}");
            public static Guid TaskOwners = new Guid("{8a77acfe-5965-4b98-851e-063a531a9681}");

            public static Guid TaskSupervisors = new Guid("{c95e9388-e937-4c9d-9f5a-ed7a4410631e}");
            public static Guid TaskRelations = new Guid("{A31B109A-3A0E-4b17-AC89-EAF6177C1D6A}");

            public static Guid TaskCategory = new Guid("{9ae6749b-c21e-44db-8b3c-6f94b3441b5a}");
            public static Guid TaskOnProject = new Guid("{236F5796-DBBE-4957-B253-C010D5546EBA}");

            public static Guid ActualStart = new Guid("{1222833e-5242-45b5-839c-199718eee45d}");
            public static Guid ActualEnd = new Guid("{dff20a1a-56fa-4a4f-91b9-a923ccf9b3d3}");
            

            public static Guid TaskManagerUser = new Guid("{C745576B-B531-435b-9F1E-863A7AC88943}");
            public static Guid TaskSupervisorUsers = new Guid("{A5B8A512-9457-4136-AF8B-2DDA14E57DF3}");
            public static Guid TaskRelationUsers = new Guid("{1A690310-14E4-4451-A70F-C0F39763D515}");

            public static Guid Assigned = new Guid("{52672DA7-DFE6-4a4f-9F9A-E18521320DD8}");

            public static Guid TaskRate = new Guid("{71e86bb9-a2ed-4ad9-8907-e6c94cfd2016}");
            public static Guid RateComment = new Guid("{575716e3-c32e-4bdf-a1a8-aba470931f1a}");
            
        }
        #endregion Task List

        #region Timesheet
        public class Timesheet
        {
            public static Guid TimesheetTask = new Guid("{7E491FE4-2DFB-401d-A873-A7E400FEFAF7}");
            public static Guid TimesheetProject = new Guid("{121FFF5C-AFB9-4122-B50E-D6278F7AF173}");
            public static Guid TypeOfWork = new Guid("{80bdaac4-ff47-479f-af69-dad6813cd0b3}");
            public static Guid WorkTime = new Guid("{8E839702-08A6-4db4-B6EC-E3135F7CF5D2}");
            public static Guid Year = new Guid("{0D025E6E-0A25-4863-95C2-BC89F78B9BCB}");
            public static Guid Month = new Guid("{47022B83-2795-4d77-9E7E-36B488232450}");
            public static Guid Week = new Guid("{3A7061A4-A9C9-4145-A9D2-E42BDD7063E4}");
            public static Guid Employee = new Guid("{DF5441DC-0E02-4f92-954D-19C93A930F86}");
            public static Guid DepartmentSiteColumn = new Guid("{8E440150-2D36-4483-A7FF-11AA0C4FE776}");
            public static Guid IsSync = new Guid("{11E6854F-F652-44af-9960-4590A811042A}");
        }
        #endregion Timesheet

        #region Stationery
     
        #endregion Stationery

        public static  Guid NavigationUrl = new Guid("{18172a87-700a-4ebc-a78d-172da8c61e1f}");
        public static Guid STT = new Guid("{DFDEFF8F-E2D3-4A7D-B34A-A204B5851FCC}");
        public static Guid NavigationKey = new Guid("{500B5DEE-9F0D-4E24-8D4B-F5C0C95279CD}");
        
        //Announcement List
        public static Guid AnnouncementStatus = new Guid("{0A245BA8-6601-483B-8B13-3EFE57D814C2}");
        public static Guid AnnouncementBranch = new Guid("{090E1790-AECD-4D89-864D-BF0F903E32AC}");
        public static Guid AnnouncementPublishDate = new Guid("{7BA23EDE-540C-45F8-B903-ABD46CF45315}");
        public static Guid AnnouncementCode = new Guid("{73A1CE5C-F18A-4974-8030-B80F8F0D4C9D}");
        public static Guid AnnouncementSigner = new Guid("{4821E8C9-3EB9-43B6-B516-A83078E05BF7}");
        public static Guid AnnouncementViewers = new Guid("{33110C63-783D-41BA-A635-69818409071F}");
        public static Guid AnnouncementCategory = new Guid("{26D3183E-C07F-488C-B3F1-78B6F5D94629}");

        public static Guid Thumbnail = new Guid("{343b2eeb-77f7-4723-bc0e-2b72ccfb2624}");

        // Comment list columns

        public static Guid ReplyTo = new Guid("{9A04E409-D277-4F84-AEB0-5F437DA0F8AD}");
        public static Guid CommentText = new Guid("{1E66B70E-1179-497A-8B73-AD139E34E11E}");
        public static Guid CommentUrl = new Guid("{504B2598-2121-44EB-A9B1-CEBBB82E0530}");
        public static Guid RepliesCount = new Guid("{E2811F1E-ED96-4440-B205-14350E02A44C}");

        public static Guid RatingUsers = new Guid("{9A2979C0-8029-4BEE-BC7F-9925F6CEF137}");
        public static Guid RatingAvg = new Guid("{636A95FE-D0A2-457F-82AE-1CB4D2E9932E}");

        // News list columns
        public static Guid NewsShortDescription = new Guid("{2CC35F31-20AE-45D7-A987-42DCAE98B5DD}");
        public static Guid NewsContents = new Guid("{7d84c9db-5df8-4f1c-95a6-8dc00117c1cf}");
        public static Guid NewsThumbnail1 = new Guid("{343b2eeb-77f7-4723-bc0e-2b72ccfb2624}");
        public static Guid NewsApprover = new Guid("{b538c4ff-3102-462f-8db5-ea1144996d9f}");
        public static Guid NewsViewCount = new Guid("{7BAF75F1-D250-4368-A941-E2514AE64D5A}");
        public static Guid NewsIsHotNews = new Guid("{857C54F5-BDFF-4FB8-BB49-1D1618E63B3D}");

        // Company Calendar list columns
        public static Guid ComCalendarTitle = new Guid("{fa564e0f-0c70-4ab9-b863-0177e6ddd247}");
        public static Guid ComCalendarLocation = new Guid("{288f5f32-8462-4175-8f09-dd7ba29359a9}");
        public static Guid ComCalendarAttendees = new Guid("{9E13F687-DD1C-44ED-89A5-83BF27A89959}");
        public static Guid ComCalendarEventDate = new Guid("{64cd368d-2f95-4bfc-a1f9-8d4324ecb007}");
        public static Guid ComCalendarEndDate = new Guid("{2684f9f2-54be-429f-ba06-76754fc056bf}");
        public static Guid ComCalendarDescription = new Guid("{9da97a8a-1da5-4a77-98d3-4bc10456e700}");

        public static Guid RelatedContent = new Guid("{05DE4EEF-6D53-48AF-A0B0-4F1E6BBE7ACD}");


        //Official Document columns
        public class OfficialDocument {
            public static Guid OfficialNumber = new Guid("{C05B4594-7C51-4786-BA6A-8670007E7931}");
            public static Guid ReferenceNumber = new Guid("{49D52686-B728-4BC5-AFF7-59D59486D99B}");
            public static Guid IncomingNumber = new Guid("{1DA145E0-06CE-40F6-B680-FC481B5AA07D}");
            public static Guid Description = new Guid("{681E15F4-B236-403D-8691-9A1D7A8174DD}");
            public static Guid IssuedDate = new Guid("{D0A4C0F6-137B-4154-9A0F-E4899E805BFC}");
            public static Guid EffectiveDate = new Guid("{EE5E8D51-5CE0-43A6-B751-26E2D12C3883}");
            public static Guid ExpiredDate = new Guid("{7234F8E6-E625-4E90-9A28-EAED68B96910}");
            public static Guid IncomingDate = new Guid("{573FCD2A-259B-40CE-9A39-D0A5535B4C2B}");
            public static Guid Effect = new Guid("{38678740-7A35-417B-9CC0-5AFDA20661FC}");
            public static Guid Signer = new Guid("{D7FF7C5F-EB5B-4F31-81C3-FBFA6FC92E96}");
            public static Guid Recipient = new Guid("{599D9D7D-6817-4AA9-A112-428C667126F1}");
            public static Guid LegislationType = new Guid("{E6488D52-53DE-4263-9381-61784A1C9962}");
            public static Guid IncomingDocumentType = new Guid("{4079BDC3-A1AC-426A-B109-EBE130300106}");
            public static Guid Organization = new Guid("{698CAE3B-B9FA-4B6D-BC72-73925FF61A7D}");
            public static Guid RelatedForms = new Guid("{034ADDC6-C259-4F6A-85D7-A98AE4DD608E}");
            public static Guid RelatedDocuments = new Guid("{F69ACCA9-7C0F-434C-B6BD-212F1D9AD8EC}");
            public static Guid ConfidentialityLevel = new Guid("{8BF5955C-6C4C-406A-8FB9-1B9D41611346}");
            public static Guid PriorityLevel = new Guid("{00670A3D-BE24-40B8-B6AD-31C4C58074D8}");
            public static Guid TargetUser = new Guid("{CE806CF2-9AA5-45D3-966E-ADD0E86110CB}");
            public static Guid Approver = new Guid("{B538C4FF-3102-462F-8DB5-EA1144996D9F}");

            public static Guid ApproverNote = new Guid("{B533A9A0-B014-4E59-B161-FDD7F1523A14}");
            public static Guid RecipientNote = new Guid("{E2EA3E48-21DD-423B-975A-1396178C9618}");
            public static Guid FocalPointUnitNote = new Guid("{A953DC5F-A802-4730-AB39-8A5FA7A87A68}");
            public static Guid ImplementationUnitNote = new Guid("{B3696CEE-5A57-48A9-91DD-6AF94114A723}");

            public static Guid ApprovedDate = new Guid("{68D36015-62AF-45F7-887D-CF6B463888FC}");
            public static Guid FocalPointUnit = new Guid("{E30212E4-153D-429C-81DB-CD6D355CB22B}");
            public static Guid ImplementationUnit = new Guid("{90AF6566-C9F5-4E0A-B361-44F6E5A54813}");
        }

        public class Building
        {
            public static Guid Address = new Guid("{4eb3744a-6365-4b01-90bf-3555d7b2151c}");
        }

        public class Room {
            public static Guid Floor = new Guid("{7c50edd5-9223-41de-8928-ec52f7ccd6e1}");
            public static Guid Building = new Guid("{A81FE7B0-240B-4E27-B072-880E1796AF0B}");
        }
        
    }
}
