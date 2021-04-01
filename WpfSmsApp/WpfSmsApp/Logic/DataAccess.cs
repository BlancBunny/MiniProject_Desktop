using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSmsApp.Model;

namespace WpfSmsApp.Logic
{
    public class DataAccess
    {
        /// <summary>
        /// SELECT
        /// </summary>
        /// <returns>유저 리스트</returns>
        public static List<User> GetUsers()
        {
            List<User> users;
            using (var ctx = new SMSEntities())
            {
                users = ctx.User.ToList();
            }
            return users;
        }

        /// <summary>
        /// INSERT, UPDATE
        /// </summary>
        /// <param name="user"></param>
        /// <returns>엔티티 프레임워크 변경 여부 (0 or 1+)</returns>
        public static int SetUser(User user)
        {
            using (var ctx = new SMSEntities())
            {
                ctx.User.AddOrUpdate(user);
                return ctx.SaveChanges(); // commit
            }
        }

        
        public static List<Store> GetStores()
        {
            List<Store> stores;
            using (var ctx = new SMSEntities())
            {
                stores = ctx.Store.ToList();
            }
            return stores;
        }

        public static int SetScore(Store store)
        {
            using (var ctx = new SMSEntities())
            {
                ctx.Store.AddOrUpdate(store);
                return ctx.SaveChanges(); // commit
            }
        }


        public static List<Stock> GetStocks()
        {
            List<Stock> stocks;
            using (var ctx = new SMSEntities())
            {
                stocks = ctx.Stock.ToList();
            }
            return stocks;
        }

        public static int SetScock(Stock stock)
        {
            using (var ctx = new SMSEntities())
            {
                ctx.Stock.AddOrUpdate(stock);
                return ctx.SaveChanges(); // commit
            }
        }
    }
}
