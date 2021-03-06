using System.Collections.Generic;
using WebAPI.Models.DBObjects;

namespace WebAPI.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<WebAPI.Models.DBObjects.Database>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        /// <summary>
        /// This is the seed method which creates all the needed static data in the datbase, such as items, categories, groups, roles and delivery types.
        /// If it is run, it will also delete all currently existing data in the database.
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(WebAPI.Models.DBObjects.Database context)
        {
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'IF OBJECT_ID(''?'') NOT IN (ISNULL(OBJECT_ID(''[dbo].[__MigrationHistory]''),0)) DELETE FROM ?'");
            context.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");

            var deliveryTypes = new List<DeliveryType>()
            {
                new DeliveryType()
                {
                    OrderType = 0,
                    Type = "For delivery"
                },
                new DeliveryType()
                {
                    OrderType = 0,
                    Type = "For serving"
                },
                new DeliveryType()
                {
                    OrderType = 0,
                    Type = "For takeaway"
                },
                new DeliveryType()
                {
                    OrderType = 0,
                    Type = "Bulk order"
                }
            };
            foreach (var dt in deliveryTypes)
            {
                context.DeliveryTypes.AddOrUpdate(dt);
            }
            


            var roles = new List<Role>()
            {
                new Role(){ Name = "Manager"},
                new Role(){ Name = "Chef"},
                new Role() {Name = "Waiter"},
                new Role() {Name = "Delivery"}
            };

            foreach (var r in roles)
            {
                context.Roles.AddOrUpdate(r);
            }
            

            var groups = new List<Group>()
            {
                new Group() {Name = "only pending"},
                new Group() {Name = "Edit events"},
                new Group() {Name = "Hidden edit events" }
            };

            foreach (var g in groups)
            {
                context.Groups.AddOrUpdate(g);
            }
            

            var cat0 = new Category()
            {

                Name = "Burger"
            };
            var cat1 = new Category()
            {

                Name = "Pizza"
            };
            var cat2 = new Category()
            {

                Name = "Drink"
            };
            context.Categories.AddOrUpdate(
                a => a.Name,
                cat0);
            context.Categories.AddOrUpdate(
                a => a.Name,
                cat2);
            context.Categories.AddOrUpdate(
                a => a.Name,
                cat1);

            var item0 = new Item()
            {

                Name = "Koebenhavn",
                Category = cat0,
                Price = 50.50,
                Description = "Inspired by the culture in Copenhagen"
            };
            var item1 = new Item()
            {

                Name = "Liverpool",
                Category = cat0,
                Price = 60.00,
                Description = "Inspired by the culture in Liverpool"
            };
            var item2 = new Item()
            {

                Name = "Kreta",
                Category = cat0,
                Price = 50.00,
                Description = "Inspired by the culture on Kreta"
            };
            var item3 = new Item()
            {

                Name = "Lone Star",
                Category = cat0,
                Price = 100.00,
                Description = "A good but very lonely burger"
            };
            var item4 = new Item()
            {

                Name = "Cola",
                Category = cat2,
                Price = 25.00,
                Description = "qwe"
            };
            var item5 = new Item()
            {

                Name = "Fanta",
                Category = cat2,
                Price = 25.00,
                Description = "qwe"
            };
            var item6 = new Item()
            {

                Name = "Sprite",
                Category = cat2,
                Price = 25.00,
                Description = "qwe"
            };
            context.Items.AddOrUpdate(
              a => a.Name,
              item0);
            context.Items.AddOrUpdate(
              a => a.Name,
              item1);
            context.Items.AddOrUpdate(
              a => a.Name,
              item2);
            context.Items.AddOrUpdate(
              a => a.Name,
              item3);
            context.Items.AddOrUpdate(
              a => a.Name,
              item4);
            context.Items.AddOrUpdate(
              a => a.Name,
              item5);
            context.Items.AddOrUpdate(
                a => a.Name,
                item6);
        }
    }
}
