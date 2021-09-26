﻿using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Helpers
{
    public class TagHelper
    {
        public static IEnumerable<Tag> GetOrCreateTags(IRepository<Tag> repository, string tagsString)
        {
            List<Tag> tags = new();

            if (string.IsNullOrWhiteSpace(tagsString))
            {
                return tags;
            }

            var tagStrings = Regex.Split(tagsString.ToLower(), @"\W+");
            var existsTags = repository.Find(x => tagStrings.Contains(x.Name)).ToList();

            repository.GetCurrentTransaction(out ITransaction transaction, out bool isNew);
            foreach (var tagString in tagStrings)
            {
                if (string.IsNullOrEmpty(tagString))
                {
                    continue;
                }
                Tag tag = existsTags.Find(x => x.Name == tagString);
                if (tag == null)
                {
                    tag = new(tagString);
                    tag.Id = repository.Save(tag);
                }
                tags.Add(tag);
            }
            if (isNew)
            {
                transaction.Commit();
            }

            return tags;
        }
    }
}