using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RTS
{
    public static class SqlExec
    {
        public static string DbConnectionString { get; set; }

        public static List<Model.Category> GetCategories()
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Select("*");
                sql.From("Category");
                sql.OrderBy("SortId ASC, CategoryName ASC");
                var results = db.Fetch<Model.Category>(sql);
                return results;
            }
        }

        public static Model.Category GetCategoryByCategoryCode(string categoryCode)
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Select("*");
                sql.From("Category");
                sql.Where("CategoryCode = @0", categoryCode);
                sql.OrderBy("SortId ASC, CategoryName ASC");
                var results = db.Fetch<Model.Category>(sql);
                if (results.Count > 0)
                    return results[0];
                else
                    return null;
            }
        }
        public static int GetToneCount()
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Select("COUNT(*) AS ToneCount");
                sql.From("Tone");
                var results = db.Fetch<dynamic>(sql);
                if (results.Count > 0)
                    return results[0].ToneCount;
                else
                    return 0;

            }
        }

        public static List<Model.Tone> GetTopTenTones()
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Select("TOP 10 *");
                sql.From("Tone");
                sql.OrderBy("Counter DESC");
                var results = db.Fetch<Model.Tone>(sql);
                return results;
            }
        }

        public static List<Model.Tone> GetTonesByCategory(string category)
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Select("*");
                sql.From("Tone");
                sql.Where("Category LIKE @0", "%-" + category + "-%");
                sql.OrderBy("Artist ASC, Title ASC");
                var results = db.Fetch<Model.Tone>(sql);
                return results;
            }
        }

        public static void IncrementDownloadCount(int toneId)
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Select("*");
                sql.From("Tone");
                sql.Where("ToneId = @0", toneId);
                var results = db.Fetch<Model.Tone>(sql);
                if (results.Count > 0)
                {
                    results[0].Counter = results[0].Counter + 1;
                    db.Update(results[0]);
                }
            }
        }
        public static List<Model.Tone> GetTonesBySearch(string search)
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Select("*");
                sql.From("Tone");
                sql.Where("Artist LIKE @0 OR Title LIKE @0 OR ToneId = CASE WHEN ISNUMERIC(@1) = 1 THEN @1 ELSE -1 END", "%" + search + "%", search);
                sql.OrderBy("Artist ASC, Title ASC");
                var results = db.Fetch<Model.Tone>(sql);
                return results;
            }
        }

        public static Model.Tone GetToneById(int id)
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Select("*");
                sql.From("Tone");
                sql.Where("ToneId = @0", id);
                var results = db.Fetch<Model.Tone>(sql);
                if (results.Count > 0)
                {
                    return results[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public static Model.Setting GetSetting(string settingName)
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Select("*");
                sql.From("Setting");
                sql.Where("SettingName = @0", settingName);
                var results = db.Fetch<Model.Setting>(sql);
                if (results.Count > 0)
                {
                    return results[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool UpdateHitCounter(string ipAddress)
        {
            var isNewDay = CheckForNewDay();
            if (Properties.Settings.Default.ResetVisitorsDaily && isNewDay)
            {
                ResetVisitors();
            }

            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Select("*");
                sql.From("Visitor");
                sql.Where("IPAddress = @0", ipAddress);
                var results = db.Fetch<Model.Visitor>(sql);
                if (results.Count > 0)
                {
                    results[0].LastVisit = DateTime.Now;
                    db.Update(results[0]);
                    IncrementHitCounterTotal(false);

                }
                else
                {
                    InsertIPAddress(ipAddress);
                    IncrementHitCounterTotal(true);
                }
            }

            return true;
        }

        private static void InsertIPAddress(string ipAddress)
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var visitor = new Model.Visitor();
                visitor.IPAddress = ipAddress;
                var visitTime = DateTime.Now;
                visitor.FirstVisit = visitTime;
                visitor.LastVisit = visitTime;
                db.Insert(visitor);
            }
        }

        private static void IncrementHitCounterTotal(bool isUnique)
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Append("UPDATE Setting SET");
                sql.Append("SettingValue = CAST(CAST(SettingValue AS int) + 1 AS nvarchar(50))");
                sql.Append("WHERE SettingName = 'PageViews'");
                var pageViewResults = db.Execute(sql);
                if (isUnique)
                {
                    sql.Append("UPDATE Setting SET");
                    sql.Append("SettingValue = CAST(CAST(SettingValue AS int) + 1 AS nvarchar(50))");
                    sql.Append("WHERE SettingName = 'UniqueHits'");
                    var uniqueHitResults = db.Execute(sql);
                }
            }
        }

        private static bool CheckForNewDay()
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Select("*");
                sql.From("Setting");
                sql.Where("SettingName = 'CurrentDate'");
                var results = db.Fetch<Model.Setting>(sql);
                if (results.Count > 0 && DateTime.Parse(results[0].SettingValue).ToShortDateString() != DateTime.Now.ToShortDateString())
                {
                    sql = new NPoco.Sql();
                    sql.Append("UPDATE Setting SET");
                    sql.Append("SettingValue = @0", DateTime.Now.ToString("yyyy-MM-dd"));
                    sql.Append("WHERE SettingName = 'CurrentDate'");
                    db.Execute(sql);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static void ResetVisitors()
        {
            using (var db = new NPoco.Database(DbConnectionString, Properties.Settings.Default.DbProviderName))
            {
                var sql = new NPoco.Sql();
                sql.Append("DELETE FROM Visitor");
                db.Execute(sql);

            }
        }
    }
}