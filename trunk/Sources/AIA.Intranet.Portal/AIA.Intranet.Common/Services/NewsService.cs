﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities.Camlex;

namespace AIA.Intranet.Common.Services
{
    public class NewsService
    {
        public static List<Model.Entities.NewsItem> GetLastestNews(SPWeb web, int itemCount, bool hotNewsOnly)
        {
            CAMLSiteQuery<NewsItem> query = new CAMLSiteQuery<NewsItem>(ListTemplateIds.NEWS_TEMPLATE_ID, web);

            string caml = string.Empty;

            if (hotNewsOnly)
            {
                 caml = Camlex.Query()
                              .Where(x => (bool)x["IsHotNews"] == true &&
                                          x[SPBuiltInFieldId._ModerationStatus] == (DataTypes.ModStat)"0")
                              .OrderBy(x => x[SPBuiltInFieldId.Created] as Camlex.Desc)
                              .ToString();
            }

            return query.ExecuteListQuery(caml, itemCount);
        }

        public static List<Model.Entities.NewsItem> GetLastestNews(int itemCount)
        {
            return GetLastestNews(SPContext.Current.Web, itemCount, false);
        }

        public static List<Model.Entities.NewsItem> GetLastestNews(int itemCount, bool hotNewsOnly)
        {
            return GetLastestNews(SPContext.Current.Web, itemCount, hotNewsOnly);
        }

        public static List<Model.Entities.NewsItem> GetLatestNewsByCategory(SPList list, int itemCount)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            string caml = string.Empty;
            caml = Camlex.Query()
                         .Where(x => x[SPBuiltInFieldId._ModerationStatus] == (DataTypes.ModStat)"0")
                         .OrderBy(x => x[SPBuiltInFieldId.Created] as Camlex.Desc)
                         .ToString();

            return query.ExecuteListQuery(caml, itemCount);
        }

        public static List<Model.Entities.NewsItem> GetNewsByCategory(SPList list, int itemCount, string pagingInfo, out SPListItemCollection collection)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            string caml = string.Empty;
            caml = Camlex.Query()
                         .Where(x => x[SPBuiltInFieldId._ModerationStatus] == (DataTypes.ModStat)"0")
                         .OrderBy(x => x[SPBuiltInFieldId.Created] as Camlex.Desc)
                         .ToString();

            return query.ExecuteListQuery(caml, itemCount, pagingInfo, out collection);
        }

        public static Model.Entities.NewsItem GetNewsById(SPList list, int id)
        { 
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            return query.GetItemById(id);
        }

        public static Model.Entities.NewsItem GetLatestNewsInCategory(SPList list)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            string caml = string.Empty;
            caml = Camlex.Query()
                         .Where(x => x[SPBuiltInFieldId._ModerationStatus] == (DataTypes.ModStat)"0")
                         .OrderBy(x => x[SPBuiltInFieldId.Created] as Camlex.Desc)
                         .ToString();

            return query.ExecuteListQuery(caml, 1).FirstOrDefault();
        }

        public static Model.Entities.NewsItem CreateInstance(SPList list, SPListItem item)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            return query.CreateInstance(item);
        }

        public static Model.Entities.NewsItem CreateInstance(SPListItem item)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(item.ParentList);

            return query.CreateInstance(item);
        }

        public static List<Model.Entities.NewsItem> GetOtherNews(SPList list, int exceptId, int itemCount)
        {
            CAMLListQuery<NewsItem> query = new CAMLListQuery<NewsItem>(list);

            string caml = string.Empty;
            caml = Camlex.Query()
                         .Where(x => (int)x[SPBuiltInFieldId.ID] != exceptId && x[SPBuiltInFieldId._ModerationStatus] == (DataTypes.ModStat)"0")
                         .OrderBy(x => x[SPBuiltInFieldId.Created] as Camlex.Desc)
                         .ToString();

            return query.ExecuteListQuery(caml, itemCount);
        }
    }
}