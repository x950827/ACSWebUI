using System.Collections.Generic;
using System.Linq;
using ACSWebUI.Common.Entity;
using ACSWebUI.Common.Model;

namespace ACSWebUI.Database.Extensions
{
    public static class PassageExtension
    {
        public static Passage[] FromTables(this IEnumerable<PassageEntity> passages)
        {
            return passages.Select(FromTable).ToArray();
        }

        public static Passage FromTable(this PassageEntity passage)
        {
            if (passage == null)
                return null;

            return new Passage
            {
                skip_id = passage.SkipId,
                date = passage.Date,
            };
        }

        public static PassageEntity[] ToTables(this IEnumerable<Passage> passages)
        {
            return passages.Select(ToTable).ToArray();
        }

        public static PassageEntity ToTable(this Passage passage)
        {
            if (passage == null)
                return new PassageEntity();

            return new PassageEntity
            {
                SkipId = passage.skip_id,
                Date = passage.date,
            };
        }
    }
}
